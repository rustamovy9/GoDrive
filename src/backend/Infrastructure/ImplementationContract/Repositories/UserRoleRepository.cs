using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class UserRoleRepository(DataContext dbContext)
    : GenericRepository<UserRole>(dbContext), IUserRoleRepository;