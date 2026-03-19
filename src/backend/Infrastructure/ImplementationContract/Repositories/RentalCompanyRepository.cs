using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class RentalCompanyRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<RentalCompany>(dbContext, localizer), IRentalCompanyRepository;
