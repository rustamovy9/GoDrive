using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class RoleRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<Role>(dbContext, localizer), IRoleRepository;
