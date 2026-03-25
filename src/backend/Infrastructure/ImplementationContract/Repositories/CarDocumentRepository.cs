using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class CarDocumentRepository(DataContext dbContext)
    : GenericRepository<CarDocument>(dbContext), ICarDocumentRepository;
