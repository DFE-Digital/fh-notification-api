﻿using FamilyHubs.Notification.Core.Interfaces.Commands;
using FamilyHubs.Notification.Data.NotificationServices;
using FamilyHubs.Notification.Data.Shared;
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
    private readonly IEmailSender _emailSender;
    private readonly ILogger<CreateNotificationCommandHandler> _logger;

    public CreateNotificationCommandHandler(IEmailSender emailSender, ILogger<CreateNotificationCommandHandler> logger)
    {
        _emailSender = emailSender;
        _logger = logger;

    }
    public async Task<bool> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _emailSender.SendEmailAsync(request.MessageDto);
            return true;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred sending notification. {exceptionMessage}", ex.Message);
            throw;
        }
    }
}
