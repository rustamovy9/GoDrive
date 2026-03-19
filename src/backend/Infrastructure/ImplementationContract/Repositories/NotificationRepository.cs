using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class NotificationRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<Notification>(dbContext, localizer), INotificationRepository;
