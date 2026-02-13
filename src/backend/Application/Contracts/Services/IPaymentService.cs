using Application.DTO_s;
using Application.Extensions.ResultPattern;
using Domain.Enums;

namespace Application.Contracts.Services;

public interface IPaymentService
{
    Task<BaseResult> CreateAsync(PaymentCreateInfo createInfo);

    Task<Result<IEnumerable<PaymentReadInfo>>> GetByBookingIdAsync(int bookingId);

    Task<BaseResult> UpdateStatusAsync(int paymentId, PaymentStatus status);
}