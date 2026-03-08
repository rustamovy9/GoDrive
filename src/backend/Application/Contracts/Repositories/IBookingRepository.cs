using Application.Contracts.Repositories.BaseRepository;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IBookingRepository : IGenericRepository<Booking>
{
    Task<int> GetActiveRentals(int ownerId);
    Task<decimal> GetOwnerMonthlyEarnings(int ownerId);
}