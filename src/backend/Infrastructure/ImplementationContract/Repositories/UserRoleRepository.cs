using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class UserRoleRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<UserRole>(dbContext, localizer), IUserRoleRepository;
