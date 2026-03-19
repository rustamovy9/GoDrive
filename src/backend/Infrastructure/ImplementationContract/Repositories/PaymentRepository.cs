using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class PaymentRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<Payment>(dbContext, localizer), IPaymentRepository;
