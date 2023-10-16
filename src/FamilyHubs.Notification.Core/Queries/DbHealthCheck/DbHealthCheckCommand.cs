using Ardalis.GuardClauses;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.Repository;
using MediatR;

namespace FamilyHubs.Notification.Core.Queries.DbHealthCheck;

public class DbHealthCheckCommand : IRequest<bool>
{
    public DbHealthCheckCommand()
    {
        
    }
}

public class DbHealthCheckCommandHandler : IRequestHandler<DbHealthCheckCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DbHealthCheckCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

   
    public async Task<bool> Handle(DbHealthCheckCommand request, CancellationToken cancellationToken)
    {
        bool result = await Task.Run(() => {
            return CheckConnection();
        });

        if (!result)
        {
            throw new NotFoundException(nameof(SentNotification), "");
        }

        return result;
    }

    private bool CheckConnection()
    {
        try
        {
            return _context.Database.CanConnect();
        }
        catch
        {
            return false;
        }
        
    }
}
