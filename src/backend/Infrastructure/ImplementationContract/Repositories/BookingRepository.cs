using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories;

public class BookingRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<Booking>(dbContext, localizer), IBookingRepository
{
    public async Task<int> GetActiveRentals(int ownerId)
    {
        return await dbContext.Bookings
            .Where(x =>
                x.Car.OwnerId == ownerId &&
                x.BookingStatus == BookingStatus.Confirmed)
            .CountAsync();
    }

    public async Task<decimal> GetOwnerMonthlyEarnings(int ownerId)
    {
        var start = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

        var earnings = await dbContext.Bookings
            .Where(x =>
                x.Car.OwnerId == ownerId &&
                x.BookingStatus == BookingStatus.Confirmed &&
                x.CreatedAt >= start)
            .Select(x => x.TotalPrice)
            .DefaultIfEmpty(0)
            .SumAsync();

        return earnings;
    }

    public async Task<int> GetOwnerTotalBookings(int ownerId)
    {
        return await dbContext.Bookings
            .Where(x => x.Car.OwnerId == ownerId)
            .CountAsync();
    }
}
