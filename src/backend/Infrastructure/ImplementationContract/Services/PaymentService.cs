using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class PaymentService(
    IPaymentRepository repository,
    IBookingRepository bookingRepository,
    INotificationService notificationService,
    ICarRepository carRepository) : IPaymentService
{
    public async Task<BaseResult> CreateAsync(PaymentCreateInfo createInfo)
    {
        if (createInfo.Amount <= 0)
            return BaseResult.Failure(
                Error.BadRequest("Amount must be greater than zero."));

        var bookingRes = await bookingRepository.GetByIdAsync(createInfo.BookingId);

        if (!bookingRes.IsSuccess || bookingRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Booking not found"));

        var booking = bookingRes.Value;

        var existing = repository.Find(p => p.BookingId == createInfo.BookingId);

        if (existing.IsSuccess && await existing.Value!.AnyAsync())
            return BaseResult.Failure(
                Error.Conflict("Payment already exists for this booking"));

        var payment = createInfo.ToEntity();

        var result = await repository.AddAsync(payment);

        if (!result.IsSuccess)
            return BaseResult.Failure(result.Error);

        await SendPaymentCreatedNotifications(booking);

        return BaseResult.Success();
    }

    public async Task<Result<IEnumerable<PaymentReadInfo>>> GetByBookingIdAsync(int bookingId)
    {
        var res = repository.Find(x => x.BookingId == bookingId);

        if (!res.IsSuccess)
            return Result<IEnumerable<PaymentReadInfo>>.Failure(res.Error);

        var data = await res.Value!
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.ToRead())
            .ToListAsync();

        return Result<IEnumerable<PaymentReadInfo>>.Success(data);
    }

   public async Task<BaseResult> UpdateStatusAsync(int paymentId, PaymentStatus newStatus)
    {
        var paymentRes = await repository.GetByIdAsync(paymentId);

        if (!paymentRes.IsSuccess || paymentRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Payment not found"));

        var payment = paymentRes.Value;

        if (payment.Status == newStatus)
            return BaseResult.Success();

        if (payment.Status == PaymentStatus.PaidOffline)
            return BaseResult.Failure(
                Error.BadRequest("Paid payment cannot be modified"));

        payment.Status = newStatus;
        payment.UpdatedAt = DateTimeOffset.UtcNow;
        payment.Version++;

        var updateResult = await repository.UpdateAsync(payment);

        if (!updateResult.IsSuccess)
            return BaseResult.Failure(updateResult.Error);

        await UpdateBookingStatusFromPayment(payment, newStatus);
        await SendPaymentNotifications(payment, newStatus);

        return BaseResult.Success();
    }

    private async Task UpdateBookingStatusFromPayment(Payment payment, PaymentStatus newStatus)
    {
        var bookingRes = await bookingRepository.GetByIdAsync(payment.BookingId);

        if (!bookingRes.IsSuccess || bookingRes.Value is null)
            return;

        var booking = bookingRes.Value;

        if (booking.BookingStatus == BookingStatus.Completed ||
            booking.BookingStatus == BookingStatus.Cancelled ||
            booking.BookingStatus == BookingStatus.Rejected)
            return;

        if (newStatus == PaymentStatus.AgreedOffline ||
            newStatus == PaymentStatus.PaidOffline)
        {
            booking.BookingStatus = BookingStatus.Confirmed;
        }
        else if (newStatus == PaymentStatus.PendingAgreement)
        {
            booking.BookingStatus = BookingStatus.Pending;
        }

        booking.UpdatedAt = DateTimeOffset.UtcNow;
        booking.Version++;

        await bookingRepository.UpdateAsync(booking);
    }

    private async Task SendPaymentNotifications(Payment payment, PaymentStatus newStatus)
    {
        var bookingRes = await bookingRepository.GetByIdAsync(payment.BookingId);
        if (!bookingRes.IsSuccess || bookingRes.Value is null)
            return;

        var booking = bookingRes.Value;

        // 🔔 Клиенту
        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                booking.UserId,
                "Payment status updated",
                $"Your payment status changed to {newStatus}."
            ));

        // 🔔 Владельцу
        var carRes = await carRepository.GetByIdAsync(booking.CarId);
        if (carRes.IsSuccess && carRes.Value is not null)
        {
            await notificationService.CreateAsync(
                new NotificationCreateInfo(
                    carRes.Value.OwnerId,
                    "Payment status updated",
                    $"Payment for booking #{booking.Id} changed to {newStatus}."
                ));
        }
    }
    
    private async Task SendPaymentCreatedNotifications(Booking booking)
    {
        // Клиенту
        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                booking.UserId,
                "Payment created",
                $"Payment for booking #{booking.Id} has been created."
            ));

        // Владельцу
        var carRes = await carRepository.GetByIdAsync(booking.CarId);

        if (carRes.IsSuccess && carRes.Value is not null)
        {
            await notificationService.CreateAsync(
                new NotificationCreateInfo(
                    carRes.Value.OwnerId,
                    "New payment",
                    $"User created payment for booking #{booking.Id}."
                ));
        }
    }
}