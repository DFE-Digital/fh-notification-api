using FamilyHubs.Notification.Data.Shared;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IEmailSender
{
    Task SendEmailAsync(MessageDto messageDto);
}
