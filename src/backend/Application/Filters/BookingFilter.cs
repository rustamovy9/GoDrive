using Domain.Common;
using Domain.Enums;

namespace Application.Filters;

public record BookingFilter(
    int? UserId,
    int? CarId,
    
    BookingStatus? Status,
    PaymentStatus? PaymentStatus,
    PaymentMethod? PaymentMethod,
    
    int? PickupLocationId,
    int? DropOffLocationId,
    
    DateTimeOffset? StartDate,
    DateTimeOffset? EndDate,
    
    decimal? MinPrice,
    decimal? MaxPrice,
    
    bool? IsContactShared) : BaseFilter;