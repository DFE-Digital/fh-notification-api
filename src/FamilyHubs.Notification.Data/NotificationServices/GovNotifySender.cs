using FamilyHubs.Notification.Api.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Exceptions;
using Notify.Interfaces;

namespace FamilyHubs.Notification.Data.NotificationServices;

public interface IServiceNotificationClient : IAsyncNotificationClient
{
    ApiKeyType ApiKeyType { get; }
}

//public interface IConnectNotificationClient : IAsyncNotificationClient
//{
//}

//public interface IManageNotificationClient : IAsyncNotificationClient
//{
//}

public class ServiceNotificationClient : NotificationClient, IServiceNotificationClient
{
    public ApiKeyType ApiKeyType { get; private set; }

    //todo: ConnectNotificationClient is transient, so we'll have a new HttpClient each time, but HttpClient should be a singleton
    public ServiceNotificationClient(ApiKeyType apiKeyType, string apiKey)
            : base(new HttpClientWrapper(new HttpClient()), apiKey)
    {
        ApiKeyType = apiKeyType;
    }

    //public ServiceNotificationClient(IHttpClient client, string apiKey)
    //    : base(client, apiKey)
    //{
    //}
}

//public class ManageNotificationClient : NotificationClient, IManageNotificationClient
//{
//    public string Name => "Manage";

//    public ManageNotificationClient(string apiKey)
//            : base(new HttpClientWrapper(new HttpClient()), apiKey)
//    {
//    }

//    public ManageNotificationClient(IHttpClient client, string apiKey)
//        : base(client, apiKey)
//    {
//    }

//}

//public class ConnectNotifySender : GovNotifySender, IConnectSender
//{
//    public ConnectNotifySender(
//        IEnumerable<IAsyncNotificationClient> notificationClients,
//        IOptions<GovNotifySetting> govNotifySettings,
//        ILogger<ConnectNotifySender> logger)
//        : base(notificationClients, govNotifySettings, logger)
//    {
//        NotificationClient = notificationClients.FirstOrDefault(x => x.GetType() == typeof(ConnectNotificationClient));
//        if (NotificationClient == null)
//        {
//            throw new InvalidOperationException("Connect Notification Client not found");
//        }
//    }
//}

//public class ManageNotifySender : GovNotifySender, IManageSender
//{
//    public ManageNotifySender(
//        IEnumerable<IAsyncNotificationClient> notificationClients,
//        IOptions<GovNotifySetting> govNotifySettings,
//        ILogger<ManageNotifySender> logger)
//        : base(notificationClients, govNotifySettings, logger)
//    {
//        NotificationClient = notificationClients.FirstOrDefault(x => x.GetType() == typeof(ManageNotificationClient));
//        if (NotificationClient == null)
//        {
//            throw new InvalidOperationException("Manage Notification Client not found");
//        }
//    }
//}

public interface IGovNotifySender
{
    Task SendEmailAsync(MessageDto messageDto);
}

public class GovNotifySender : IGovNotifySender
{
    private readonly IEnumerable<IServiceNotificationClient> _notificationClients;
    private readonly IOptions<GovNotifySetting> _govNotifySettings;
    private readonly ILogger _logger;

    public GovNotifySender(
        IEnumerable<IServiceNotificationClient> notificationClients,
        IOptions<GovNotifySetting> govNotifySettings,
        ILogger<GovNotifySender> logger)
    {
        _notificationClients = notificationClients;
        _govNotifySettings = govNotifySettings;
        _logger = logger;

        //todo: get all templates and cache, so that we can do error checking when sending later
    }

    public async Task SendEmailAsync(MessageDto messageDto)
    {
        var client = _notificationClients.FirstOrDefault(x => x.ApiKeyType == messageDto.ApiKeyType)
            ?? throw new InvalidOperationException($"Client for ApiKeyType {messageDto.ApiKeyType} not found");

        //do at startup and cache
        //await client.GetAllTemplatesAsync()

        var personalisation = messageDto.TemplateTokens
            .ToDictionary(pair => pair.Key, pair => (dynamic)pair.Value);

        foreach(var notification in messageDto.NotificationEmails) 
        {
            _logger.LogInformation("Sending email to: {EmailAddress}", notification);

            // make best effort to send notification to all recipients
            try
            {
                //todo: fail if messageDto.TemplateId not found, rather than have fallback template
                //todo: add dev only code to get templates from GovNotify and check exists. perhaps always do, as govuk silently ignores when template id doesn't exist
                var result = await client.SendEmailAsync(
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

