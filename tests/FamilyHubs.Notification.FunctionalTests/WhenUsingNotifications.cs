using FamilyHubs.Notification.Api.Contracts;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Xunit.Abstractions;

namespace FamilyHubs.Notification.FunctionalTests;

//https://stackoverflow.com/questions/7138935/xunit-net-does-not-capture-console-output

public class WhenUsingNotifications : BaseWhenUsingOpenReferralApiUnitTests
{
    private readonly ITestOutputHelper output;

    public WhenUsingNotifications(ITestOutputHelper output)
    {
        this.output = output;
    }

    public class ConsoleWriter : StringWriter
    {
        private readonly ITestOutputHelper output;
        public ConsoleWriter(ITestOutputHelper output)
        {
            this.output = output;
        }

        public override void WriteLine(string? value)
        {
            output.WriteLine(value);
        }
    }

    // Uncomment to run locally
    //[Theory]
    //[InlineData("ProfessionalAcceptRequest")]
    //[InlineData("ProfessionalDecineRequest")]
    //[InlineData("ProfessionalSentRequest")]
    //[InlineData("VcsNewRequest")]
    public async Task ThenSendEmailNotificationToUser(string key)
    {
        Console.SetOut(new ConsoleWriter(output));
        if (!_templates.ContainsKey(key))
        {
            return;
        }

        var command = new MessageDto
        {
            ApiKeyType = ApiKeyType.ConnectKey,
            NotificationEmails = new List<string> { _emailRecipient },
            TemplateId = _templates[key],
            TemplateTokens = new Dictionary<string, string>
            {
                { "reference number", "0001" },
                { "RequestNumber", "0001" },
                { "ServiceName", "ServiceName" },
                { "ViewConnectionRequestUrl",  "wwww.someurl.com"},
                { "Name of service", "Special Test Service" },
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
