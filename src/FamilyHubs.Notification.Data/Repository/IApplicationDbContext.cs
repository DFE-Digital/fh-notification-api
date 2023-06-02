using FamilyHubs.Notification.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Notification.Data.Repository;

public interface IApplicationDbContext
{
    DbSet<SentNotification> SentNotifications { get; }
    DbSet<TokenValue> TokenValues { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
