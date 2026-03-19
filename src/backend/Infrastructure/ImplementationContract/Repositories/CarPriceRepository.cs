using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class CarPriceRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<CarPrice>(dbContext, localizer), ICarPriceRepository;
