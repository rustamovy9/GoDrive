using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class ReviewRepository(DataContext dbContext)
    : GenericRepository<Review>(dbContext), IReviewRepository;