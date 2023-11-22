using System.Collections.Generic;
using System.Net.Http.Json;
using FamilyHubs.Notification.Api.Client.Exceptions;
using FamilyHubs.Notification.Api.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FamilyHubs.Notification.Api.Client;

#pragma warning disable S125
public class NotificationsApi : INotifications //todo: , IHealthCheckUrlGroup
{
    private readonly ILogger<NotificationsApi> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private static string? _endpoint;
    internal const string HttpClientName = "notifications";

    public NotificationsApi(IHttpClientFactory httpClientFactory, ILogger<NotificationsApi> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task SendEmailsAsync(
        IEnumerable<string> emailAddresses,
        string templateId,
        IDictionary<string, string> tokens,
        ApiKeyType apiKeyType = ApiKeyType.ConnectKey,
        CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient(HttpClientName);

        var tokenDic = tokens as Dictionary<string, string>
                       ?? tokens.ToDictionary(x => x.Key, x => x.Value);

        var message = new MessageDto
        {
            ApiKeyType = apiKeyType,
            NotificationEmails = emailAddresses as List<string> ?? emailAddresses.ToList(),
            TemplateId = templateId,
            TemplateTokens = tokenDic
        };

        string dictValues = "{" + string.Join(",", tokenDic.Select(kv => kv.Key + "=" + kv.Value).ToArray()) + "}";

        _logger.LogInformation($"Sending Notification ApiKeyType:{message.ApiKeyType.ToString()} TemplateId: {message} - Tokens: {dictValues}");

        using var response = await httpClient.PostAsJsonAsync("", message, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Email Notification Failed");
            throw new NotificationsClientException(response, await response.Content.ReadAsStringAsync(cancellationToken));
        }
    }

    internal static string GetEndpoint(IConfiguration configuration)
    {
        const string endpointConfigKey = "Notification:Endpoint";

        // as long as the config isn't changed, the worst that can happen is we fetch more than once
        return _endpoint ??= ConfigurationException.ThrowIfNotUrl(
            endpointConfigKey,
            configuration[endpointConfigKey],
            "The notifications URL", "https://localhost:7073/api/notify");
    }

    //public static Uri HealthUrl(IConfiguration configuration)
    //{
    //    return new Uri(new Uri(GetEndpoint(configuration)), "");
    //}
}
#pragma warning restore S125
