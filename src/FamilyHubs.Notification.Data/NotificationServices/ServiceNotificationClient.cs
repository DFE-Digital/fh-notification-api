using FamilyHubs.Notification.Api.Contracts;
using Notify.Client;

namespace FamilyHubs.Notification.Data.NotificationServices;

public class ServiceNotificationClient : NotificationClient, IServiceNotificationClient
{
    public ApiKeyType ApiKeyType { get; }

    public ServiceNotificationClient(ApiKeyType apiKeyType, string apiKey)
        : base(new HttpClientWrapper(new HttpClient()), apiKey)
    {
        ApiKeyType = apiKeyType;
    }
}