using FamilyHubs.Notification.Api.Contracts;
using Notify.Interfaces;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IServiceNotificationClient : IAsyncNotificationClient
{
    ApiKeyType ApiKeyType { get; }
}