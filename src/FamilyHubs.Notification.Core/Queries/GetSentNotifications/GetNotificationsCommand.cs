using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.Repository;
using FamilyHubs.Notification.Api.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Notification.Core.Queries.GetSentNotifications;

public class GetNotificationsCommand : IRequest<PaginatedList<MessageDto>>
{
    public GetNotificationsCommand(ApiKeyType? apiKeyType, NotificationOrderBy? notificationOrderBy, bool? isAscending, int? pageNumber, int? pageSize)
    {
        ApiKeyType = apiKeyType;
        OrderBy = notificationOrderBy;
        IsAscending = isAscending;
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 10;
    }

    public ApiKeyType? ApiKeyType { get; set; }
    public NotificationOrderBy? OrderBy { get; set; }
    public bool? IsAscending { get; init; }
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
            .Include(x => x.Notified)
            .AsNoTracking();

        if (entities == null)
        {
            throw new NotFoundException(nameof(SentNotification), "");
        }

        if (request.ApiKeyType != null)
        {
            entities = _context.SentNotifications.Where(x => x.ApiKeyType == request.ApiKeyType);
        }

        entities = OrderBy(entities, request.OrderBy, request.IsAscending);

        return await GetPaginatedList(request == null, entities, request?.PageNumber ?? 1, request?.PageSize ?? 10);
    }
}
