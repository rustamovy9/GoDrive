using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class CategoryRepository(DataContext dbContext)
    : GenericRepository<Category>(dbContext),ICategoryRepository;