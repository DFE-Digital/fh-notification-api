using Ardalis.GuardClauses;
using AutoMapper;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.Repository;
using FamilyHubs.Notification.Api.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FamilyHubs.Notification.Core.Queries.GetSentNotifications;

public class GetNotificationByIdCommand : IRequest<MessageDto>
{
    public GetNotificationByIdCommand(long id)
    {
        Id = id;
    }
    public long Id { get; }
}

public class GetNotificationByIdCommandHandler : IRequestHandler<GetNotificationByIdCommand, MessageDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetNotificationByIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MessageDto> Handle(GetNotificationByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.SentNotifications
            .Include(x => x.TokenValues)
            .Include(x => x.Notified)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(SentNotification), "");
        }

        return _mapper.Map<MessageDto>(entity);

    }
}
