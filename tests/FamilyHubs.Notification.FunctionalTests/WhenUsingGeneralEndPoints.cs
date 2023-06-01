using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;

namespace FamilyHubs.Notification.FunctionalTests
{
    public class WhenUsingGeneralEndPoints : BaseWhenUsingOpenReferralApiUnitTests
    {
        [Fact]
        public async Task ThenGetInfo()
        {
            string expectedStart = "Version: 1.0.0, Last Updated:";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(Client.BaseAddress + $"api/info"),
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue($"Bearer", $"{new JwtSecurityTokenHandler().WriteToken(_token)}");

            using var response = await Client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var stringResult = await response.Content.ReadAsStringAsync();

            var teststring = stringResult.Substring(1, expectedStart.Length + 1);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            ArgumentNullException.ThrowIfNull(stringResult);
            teststring.Trim().Should().Be(expectedStart.Trim());
        }
    }
}