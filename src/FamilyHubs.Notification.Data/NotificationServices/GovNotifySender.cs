using FamilyHubs.Notification.Api.Contracts;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Interfaces;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IConnectNotificationClient : IAsyncNotificationClient
{
    
}

public interface IManageNotificationClient : IAsyncNotificationClient
{
    
}

public class ConnectNotificationClient : NotificationClient, IConnectNotificationClient
{
    public string Name { get => "Connect"; }
    public ConnectNotificationClient(string apiKey)
            : base(new HttpClientWrapper(new HttpClient()), apiKey)
    {
    }

    public ConnectNotificationClient(IHttpClient client, string apiKey)
        : base(client, apiKey)
    {
    }

}

public class ManageNotificationClient : NotificationClient, IManageNotificationClient
{
    public string Name { get => "Manage"; }
    public ManageNotificationClient(string apiKey)
            : base(new HttpClientWrapper(new HttpClient()), apiKey)
    {
    }

    public ManageNotificationClient(IHttpClient client, string apiKey)
        : base(client, apiKey)
    {
    }

}

public class ConnectNotifySender : GovNotifySender, IConnectSender
{
    public ConnectNotifySender(IEnumerable<IAsyncNotificationClient> notificationClients, IOptions<GovNotifySetting> govNotifySettings)
        : base(notificationClients, govNotifySettings)
    {
        _notificationClient = notificationClients.FirstOrDefault(x => x.GetType() == typeof(ConnectNotificationClient));
        if (_notificationClient == null)
        {
            _notificationClient = notificationClients.FirstOrDefault();
        }
    }
}

public class ManageNotifySender : GovNotifySender, IManageSender
{
    public ManageNotifySender(IEnumerable<IAsyncNotificationClient> notificationClients, IOptions<GovNotifySetting> govNotifySettings)
        : base(notificationClients, govNotifySettings)
    {
        _notificationClient = notificationClients.FirstOrDefault(x => x.GetType() == typeof(ManageNotificationClient));
        if (_notificationClient == null)
        {
            _notificationClient = notificationClients.FirstOrDefault();
        }
    }
}

public class GovNotifySender
{
    private readonly IOptions<GovNotifySetting> _govNotifySettings;
    protected IAsyncNotificationClient? _notificationClient;

    public GovNotifySender(IEnumerable<IAsyncNotificationClient> notificationClients, IOptions<GovNotifySetting> govNotifySettings)
    {
        _govNotifySettings = govNotifySettings;
    }

    public async Task SendEmailAsync(MessageDto messageDto)
    {
        if (_notificationClient == null)
            return;

        Dictionary<String, dynamic> personalisation = new Dictionary<string, dynamic>();
        foreach (KeyValuePair<string, string> token in messageDto.TemplateTokens)
        {
            personalisation.Add(token.Key, token.Value);
        }

        foreach(var notification in messageDto.NotificationEmails) 
        {
            Console.WriteLine($"Sending email to: {notification}");

            await _notificationClient.SendEmailAsync(
                emailAddress: notification,
                templateId: !string.IsNullOrEmpty(messageDto.TemplateId) ? messageDto.TemplateId : _govNotifySettings.Value.TemplateId,
                personalisation: personalisation,
                clientReference: null,
                emailReplyToId: null
            );
        }

        
    }
}

