using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.Repository;
using FamilyHubs.Notification.Data.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Notification.Core.Queries.GetSentNotifications;

public class GetNotificationsCommand : IRequest<PaginatedList<MessageDto>>
{
    public GetNotificationsCommand(NotificationOrderBy? notificationOrderBy, bool? isAssending, int? pageNumber, int? pageSize)
    {
        OrderBy = notificationOrderBy;
        IsAssending = isAssending;
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 10;
    }

    public NotificationOrderBy? OrderBy { get; set; }
    public bool? IsAssending { get; init; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetNotificationsCommandHandler : GetHandlerBase, IRequestHandler<GetNotificationsCommand, PaginatedList<MessageDto>>
{
    public GetNotificationsCommandHandler(ApplicationDbContext context, IMapper mapper)
        : base(context, mapper)
    {

    }

    public async Task<PaginatedList<MessageDto>> Handle(GetNotificationsCommand request, CancellationToken cancellationToken)
    {
        var entities = _context.SentNotifications
            .Include(x => x.TokenValues)
            .AsNoTracking();

        if (entities == null)
        {
            throw new NotFoundException(nameof(SentNotification), "");
        }

        entities = OrderBy(entities, request.OrderBy, request.IsAssending);

        return await GetPaginatedList(request == null, entities, request?.PageNumber ?? 1, request?.PageSize ?? 10);
    }
}
