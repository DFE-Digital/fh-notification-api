using FamilyHubs.Notification.Api.Contracts;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IConnectSender
{
    Task SendEmailAsync(MessageDto messageDto);
}

public interface IManageSender
{
    Task SendEmailAsync(MessageDto messageDto);
}
