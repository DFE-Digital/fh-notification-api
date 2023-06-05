using FamilyHubs.Notification.Api.Contracts;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FamilyHubs.Notification.FunctionalTests;

public class WhenUsingNotifications : BaseWhenUsingOpenReferralApiUnitTests
{
    [Fact (Skip = "only needs to run when testing end to end")]
    public async Task ThenSendEmailNotificationToUser()
    {
        var command = new MessageDto
        {
            RecipientEmail = _emailRecipient,
            TemplateId = "TemplateId",
            TemplateTokens = new Dictionary<string, string>
            {
                { "key1", "value1" },
            }
        };


        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(Client.BaseAddress + "api/notify"),
            Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"),
        };

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

        using var response = await Client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var stringResult = await response.Content.ReadAsStringAsync();
        bool.TryParse(stringResult, out var result);

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        result.Should().BeTrue();
    }
}
