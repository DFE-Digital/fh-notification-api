using FamilyHubs.Notification.Api.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Exceptions;
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
    public ConnectNotifySender(
        IEnumerable<IAsyncNotificationClient> notificationClients,
        IOptions<GovNotifySetting> govNotifySettings,
        ILogger<ConnectNotifySender> logger)
        : base(notificationClients, govNotifySettings, logger)
    {
        NotificationClient = notificationClients.FirstOrDefault(x => x.GetType() == typeof(ConnectNotificationClient));
        if (NotificationClient == null)
        {
            throw new InvalidOperationException("Connect Notification Client not found");
        }
    }
}

public class ManageNotifySender : GovNotifySender, IManageSender
{
    public ManageNotifySender(
        IEnumerable<IAsyncNotificationClient> notificationClients,
        IOptions<GovNotifySetting> govNotifySettings,
        ILogger<ManageNotifySender> logger)
        : base(notificationClients, govNotifySettings, logger)
    {
        NotificationClient = notificationClients.FirstOrDefault(x => x.GetType() == typeof(ManageNotificationClient));
        if (NotificationClient == null)
        {
            throw new InvalidOperationException("Manage Notification Client not found");
        }
    }
}

public class GovNotifySender
{
    private readonly IOptions<GovNotifySetting> _govNotifySettings;
    private readonly ILogger _logger;
    protected IAsyncNotificationClient? NotificationClient;

    public GovNotifySender(
        IEnumerable<IAsyncNotificationClient> notificationClients,
        IOptions<GovNotifySetting> govNotifySettings,
        ILogger logger)
    {
        _govNotifySettings = govNotifySettings;
        _logger = logger;
    }

    public async Task SendEmailAsync(MessageDto messageDto)
    {
        if (NotificationClient == null)
            return;

        var personalisation = messageDto.TemplateTokens
            .ToDictionary(pair => pair.Key, pair => (dynamic)pair.Value);

        foreach(var notification in messageDto.NotificationEmails) 
        {
            _logger.LogInformation("Sending email to: {EmailAddress}", notification);

            // make best effort to send notification to all recipients
            try
            {
                await NotificationClient.SendEmailAsync(
                    emailAddress: notification,
                    templateId: !string.IsNullOrEmpty(messageDto.TemplateId) ? messageDto.TemplateId : _govNotifySettings.Value.TemplateId,
                    personalisation: personalisation,
                    clientReference: null,
                    emailReplyToId: null
                );
            }
            catch (NotifyClientException e)
            {
                _logger.LogError(e, "An error occurred sending notification. {ExceptionMessage}", e.Message);
            }
        }
    }
}

