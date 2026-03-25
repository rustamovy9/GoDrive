using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class PaymentRepository(DataContext dbContext)
        : GenericRepository<Payment>(dbContext),IPaymentRepository;