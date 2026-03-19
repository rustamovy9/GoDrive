using System.Linq.Expressions;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Application.Localization;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class BookingService(
    IBookingRepository repository,
    INotificationService notificationService,
    ICarPriceRepository carPriceRepository,
    ICarRepository carRepository,
    ITextLocalizer localizer) : IBookingService
{
    public async Task<Result<PagedResponse<IEnumerable<BookingReadInfo>>>> GetAllAsync(BookingFilter filter,int currentUserId,bool isAdmin)
    {
        Expression<Func<Booking, bool>> filterExpression = b =>
            (filter.Status == null || b.BookingStatus == filter.Status) &&
            (filter.PaymentStatus == null || b.PaymentStatus == filter.PaymentStatus) &&
            (filter.PaymentMethod == null || b.PaymentMethod == filter.PaymentMethod) &&
            (filter.UserId == null || b.UserId == filter.UserId) &&
            (filter.CarId == null || b.CarId == filter.CarId) &&
            (filter.PickupLocationId == null || b.PickupLocationId == filter.PickupLocationId) &&
            (filter.DropOffLocationId == null || b.DropOffLocationId == filter.DropOffLocationId) &&
            (filter.IsContactShared == null || b.IsContactShared == filter.IsContactShared) &&
            (filter.MinPrice == null || b.TotalPrice >= filter.MinPrice) &&
            (filter.MaxPrice == null || b.TotalPrice <= filter.MaxPrice) &&
            (filter.StartDate == null || b.EndDateTime > filter.StartDate) &&
            (filter.EndDate == null || b.StartDateTime < filter.EndDate);

        var request = repository.Find(filterExpression);

        if (!request.IsSuccess)
            return Result<PagedResponse<IEnumerable<BookingReadInfo>>>.Failure(request.Error);
        
        

        var query = request.Value!.AsNoTracking();

        if (!isAdmin)
        {
            query = query.Where(b =>
                b.UserId == currentUserId ||
                b.Car.OwnerId == currentUserId);
        }
        
        int count = await query.CountAsync();

        var data = await query
            .Include(x => x.Car)
            .Include(x => x.PickupLocation)
            .Include(x => x.DropOffLocation)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(x => x.ToRead())
            .ToListAsync();

        var res = PagedResponse<IEnumerable<BookingReadInfo>>
            .Create(filter.PageNumber, filter.PageSize, count, data);

        return Result<PagedResponse<IEnumerable<BookingReadInfo>>>.Success(res);
    }

    public async Task<Result<BookingReadInfo>> GetByIdAsync(int id,int currentUserId,bool isAdmin)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return Result<BookingReadInfo>.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.BookingNotFound)));
        
        if (!isAdmin && res.Value.UserId != currentUserId)
            return Result<BookingReadInfo>.Failure(ErrorFactory.Forbidden(localizer));

        return Result<BookingReadInfo>.Success(res.Value.ToRead());
    }
    
     public async Task<BaseResult> CreateAsync(BookingCreateInfo createInfo, int userId)
    {
        if (createInfo.EndDateTime <= createInfo.StartDateTime)
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.InvalidBookingPeriod)));

        Expression<Func<Booking, bool>> conflictExpr = b =>
            b.CarId == createInfo.CarId &&
            b.StartDateTime < createInfo.EndDateTime &&
            createInfo.StartDateTime < b.EndDateTime;

        var conflictQuery = repository.Find(conflictExpr);

        if (!conflictQuery.IsSuccess)
            return BaseResult.Failure(conflictQuery.Error);

        if (await conflictQuery.Value!.AnyAsync())
            return BaseResult.Failure(
                Error.Conflict(localizer.Get(TextKeys.Errors.CarAlreadyBooked)));

        var priceRes = carPriceRepository.Find(p => p.CarId == createInfo.CarId);

        if (!priceRes.IsSuccess)
            return BaseResult.Failure(priceRes.Error);

        var carPrice = await priceRes.Value!
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();

        if (carPrice is null)
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.CarPriceNotSet)));

        int days = (int)Math.Ceiling(
            (createInfo.EndDateTime - createInfo.StartDateTime).TotalDays);

        if (days <= 0)
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.InvalidBookingDuration)));

        decimal total = days * carPrice.PricePerDay;

        Booking entity = createInfo.ToEntity(userId);
        entity.TotalPrice = total;
        entity.BookingStatus = BookingStatus.Pending;

        var res = await repository.AddAsync(entity);

        if (!res.IsSuccess)
            return BaseResult.Failure(res.Error);

        // 🔔 клиенту
        await notificationService.CreateAsync(new NotificationCreateInfo(
            userId,
            localizer.Get(TextKeys.Notifications.BookingCreatedTitle),
            localizer.Get(TextKeys.Notifications.BookingCreatedMessage)
        ));

        // 🔔 владельцу машины
        var carRes = await carRepository.GetByIdAsync(createInfo.CarId);
        if (carRes.IsSuccess && carRes.Value is not null)
        {
            await notificationService.CreateAsync(new NotificationCreateInfo(
                carRes.Value.OwnerId,
                localizer.Get(TextKeys.Notifications.NewBookingTitle),
                localizer.Get(
                    TextKeys.Notifications.NewBookingMessage,
                    createInfo.StartDateTime,
                    createInfo.EndDateTime)
            ));
        }

        return BaseResult.Success();
    }

    public async Task<BaseResult> UpdateAsync(int id, BookingUpdateInfo updateInfo,int currentUserId)
    {
        var existing = await repository.GetByIdAsync(id);

        if (!existing.IsSuccess || existing.Value is null)
            return BaseResult.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.BookingNotFound)));

        var booking = existing.Value;
        
        if (booking.UserId != currentUserId)
            return BaseResult.Failure(ErrorFactory.Forbidden(localizer));

        if (booking.BookingStatus != BookingStatus.Pending)
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.UpdateOnlyPendingBookingInfo)));

        Expression<Func<Booking, bool>> conflictExpr = b =>
            b.Id != id &&
            b.CarId == updateInfo.CarId &&
            b.StartDateTime < updateInfo.EndDateTime &&
            updateInfo.StartDateTime < b.EndDateTime;

        var conflictQuery = repository.Find(conflictExpr);

        if (!conflictQuery.IsSuccess)
            return BaseResult.Failure(conflictQuery.Error);

        if (await conflictQuery.Value!.AnyAsync())
            return BaseResult.Failure(
                Error.Conflict(localizer.Get(TextKeys.Errors.CarAlreadyBooked)));

        var updated = existing.Value.ToEntity(updateInfo);

        var res = await repository.UpdateAsync(updated);

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateStatusAsync(
        int id,
        BookingUpdateStatusInfo updateStatusInfo,
        int currentUserId,
        bool isAdmin)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.BookingNotFound)));

        var booking = res.Value;

        var carRes = await carRepository.GetByIdAsync(booking.CarId);
        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.CarNotFound)));

        bool isOwner = carRes.Value.OwnerId == currentUserId;

        if (!IsAllowedTransition(
                booking.BookingStatus,
                updateStatusInfo.Status,
                currentUserId,
                booking.UserId,
                isOwner,
                isAdmin))
        {
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.InvalidBookingStatusTransition)));
        }
        if (updateStatusInfo.Status == BookingStatus.Rejected && string.IsNullOrWhiteSpace(updateStatusInfo.Reason))
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.ReasonRequired)));
        if (updateStatusInfo.Status == BookingStatus.Rejected ||
            updateStatusInfo.Status == BookingStatus.Cancelled)
        {
            booking.Comment = updateStatusInfo.Reason;
        }

        booking.BookingStatus = updateStatusInfo.Status;
        booking.UpdatedAt = DateTimeOffset.UtcNow;
        booking.Version++;

        var update = await repository.UpdateAsync(booking);

        if (!update.IsSuccess)
            return BaseResult.Failure(update.Error);

        var localizedStatus = updateStatusInfo.Status.ToLocalizedString(localizer);

        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                booking.UserId,
                localizer.Get(TextKeys.Notifications.BookingStatusUpdatedTitle),
                localizer.Get(TextKeys.Notifications.BookingStatusUpdatedMessage, localizedStatus)
            ));

        if (isOwner)
        {
            await notificationService.CreateAsync(
                new NotificationCreateInfo(
                    currentUserId,
                    localizer.Get(TextKeys.Notifications.BookingUpdatedTitle),
                    localizer.Get(TextKeys.Notifications.BookingUpdatedMessage, booking.Id, localizedStatus)
                ));
        }

        return BaseResult.Success();
    }



    public async Task<BaseResult> DeleteAsync(int id,int currentUserId,bool isAdmin)
    {
        var existing = await repository.GetByIdAsync(id);

        if (!existing.IsSuccess || existing.Value is null)
            return BaseResult.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.BookingNotFound)));
        
        if (!isAdmin && existing.Value.UserId != currentUserId)
            return BaseResult.Failure(ErrorFactory.Forbidden(localizer));
        
        if (existing.Value.BookingStatus != BookingStatus.Pending)
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.DeleteOnlyPendingBooking)));

        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
    
    
    private static bool IsAllowedTransition(
        BookingStatus current,
        BookingStatus next,
        int currentUserId,
        int bookingUserId,
        bool isOwner,
        bool isAdmin)
    {
        if (current is BookingStatus.Completed
            or BookingStatus.Cancelled
            or BookingStatus.Rejected)
            return false;

        // OWNER decisions
        if (current == BookingStatus.Pending && isOwner)
            return next is BookingStatus.Confirmed
                or BookingStatus.Rejected;

        // CLIENT decisions
        if (current == BookingStatus.Pending && currentUserId == bookingUserId)
            return next == BookingStatus.Cancelled;

        if (current == BookingStatus.Confirmed && currentUserId == bookingUserId)
            return next is BookingStatus.Completed
                or BookingStatus.Cancelled;

        // ADMIN override
        if (isAdmin)
            return next == BookingStatus.Cancelled;

        return false;
    }


}
