using FamilyHubs.Notification.Api.Contracts;

namespace FamilyHubs.Notification.Core.Interfaces.Commands;

public interface ICreateNotificationCommand
{
    MessageDto MessageDto { get; }
}

