using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Core.Queries.GetSentNotifications;
using FamilyHubs.Notification.Data.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        
        app.MapGet("api/notify", [Authorize] async (NotificationOrderBy? orderBy, bool? isAssending, int? pageNumber, int? pageSize, CancellationToken cancellationToken, ISender _mediator) =>
        {
            GetNotificationsCommand request = new(orderBy, isAssending, pageNumber, pageSize);
            var result = await _mediator.Send(request, cancellationToken);
            return result;

        }).WithMetadata(new SwaggerOperationAttribute("Get Notifications", "Get Paginated Notification List") { Tags = new[] { "Notifications" } });

    }
}
