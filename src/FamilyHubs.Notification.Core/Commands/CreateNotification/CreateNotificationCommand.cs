using AutoMapper;
using FamilyHubs.Notification.Api.Contracts;
using FamilyHubs.Notification.Core.Interfaces.Commands;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.NotificationServices;
using FamilyHubs.Notification.Data.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Notification.Core.Commands.CreateNotification;

public class CreateNotificationCommand : IRequest<bool>, ICreateNotificationCommand
{
    public CreateNotificationCommand(MessageDto messageDto)
    {
        MessageDto = messageDto;
    }

    public MessageDto MessageDto { get; }
}

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<CreateNotificationCommandHandler> _logger;

    public CreateNotificationCommandHandler(ApplicationDbContext context, IMapper mapper, IEmailSender emailSender, ILogger<CreateNotificationCommandHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _emailSender = emailSender;
        _logger = logger;

    }
    public async Task<bool> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendEmailAsync(request.MessageDto);

            var sentNotification = _mapper.Map<SentNotification>(request.MessageDto);
            if (sentNotification != null) 
            {
                _context.SentNotifications.Add(sentNotification);
                await _context.SaveChangesAsync();
            }

            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred sending notification. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}
