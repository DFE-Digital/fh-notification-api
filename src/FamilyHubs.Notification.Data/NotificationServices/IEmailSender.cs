using FamilyHubs.Notification.Api.Contracts;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IEmailSender
{
    Task SendEmailAsync(MessageDto messageDto);
}
