using FamilyHubs.Notification.Api.Contracts;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface INotifySender
{
    Task SendEmailAsync(MessageDto messageDto);
}

public interface IConnectSender : INotifySender
{
}

public interface IManageSender : INotifySender
{
}
