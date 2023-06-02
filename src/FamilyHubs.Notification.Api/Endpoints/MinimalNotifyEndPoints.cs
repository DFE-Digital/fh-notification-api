﻿using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Data.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FamilyHubs.Notification.Api.Endpoints;

public class MinimalNotifyEndPoints
{
    public void RegisterMinimalNotifyEndPoints(WebApplication app)
    {
        app.MapPost("api/notify", async ([FromBody] MessageDto request, CancellationToken cancellationToken, ISender _mediator) =>
        {
            CreateNotificationCommand command = new CreateNotificationCommand(request);
            var result = await _mediator.Send(command, cancellationToken);
            return result;

        }).WithMetadata(new SwaggerOperationAttribute("Notifications", "Send Notification") { Tags = new[] { "Notifications" } });

    }
}
