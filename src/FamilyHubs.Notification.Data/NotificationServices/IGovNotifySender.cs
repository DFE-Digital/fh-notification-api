using FamilyHubs.Notification.Api.Contracts;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IGovNotifySender
{
    Task SendEmailAsync(MessageDto messageDto);
}