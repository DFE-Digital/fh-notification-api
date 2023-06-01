using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Notification.Data.Repository;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
