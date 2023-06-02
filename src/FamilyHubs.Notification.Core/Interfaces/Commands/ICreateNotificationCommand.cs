using FamilyHubs.Notification.Data.Shared;

namespace FamilyHubs.Notification.Core.Interfaces.Commands;

public interface ICreateNotificationCommand
{
    MessageDto MessageDto { get; }
}

