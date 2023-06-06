using FamilyHubs.Notification.Api.Contracts;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FamilyHubs.Notification.FunctionalTests;

public class WhenUsingNotifications : BaseWhenUsingOpenReferralApiUnitTests
{
    // Uncomment to run locally
    //[Theory]
    //[InlineData("ProfessionalAcceptRequest")]
    //[InlineData("ProfessionalDecineRequest")]
    //[InlineData("ProfessionalSentRequest")]
    //[InlineData("VcsNewRequest")]
    public async Task ThenSendEmailNotificationToUser(string key)
    {
        if (!_templates.ContainsKey(key))
        {
            return;
        }

        var command = new MessageDto
        {
            RecipientEmail = _emailRecipient,
            TemplateId = _templates[key],
            TemplateTokens = new Dictionary<string, string>
            {
                { "reference number", "0001" },
                { "name of service", "Special Test Service" },
                { "link to specific connection request", "wwww.someurl.com" }
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
