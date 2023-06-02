using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Data.NotificationServices;
using FamilyHubs.Notification.Data.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Formats.Asn1;

namespace FamilyHubs.Notification.IntegrationTests
{
    public class WhenCreatingNotifications : DataIntegrationTestBase
    {
        [Fact]
        public async Task ThenCreateNotification()
        {
            var createNotificationCommand = new CreateNotificationCommand(new Data.Shared.MessageDto
            { 
                Id = 1,
                RecipientEmail = "someone@aol.com",
                TemplateId = "05d38535-a5c3-443e-bfde-54f2abdd5c78",
                TemplateTokens = new Dictionary<string, string>
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" },
                }

            });

            var logger = new Mock<ILogger<CreateNotificationCommandHandler>>();
            Mock<IEmailSender> mockEmailSender = new Mock<IEmailSender>();
            int sendEmailCallback = 0;
            mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<MessageDto>()))
                .Callback(() => sendEmailCallback++);

            var handler = new CreateNotificationCommandHandler(TestDbContext, Mapper, mockEmailSender.Object, logger.Object);

            //Act
            var result = await handler.Handle(createNotificationCommand, new CancellationToken());
            result.Should().BeTrue();
            var actualNotification = TestDbContext.SentNotifications.SingleOrDefault(x => x.Id == createNotificationCommand.MessageDto.Id);
            ArgumentNullException.ThrowIfNull(actualNotification);
            actualNotification.RecipientEmail.Should().Be(createNotificationCommand.MessageDto.RecipientEmail);
            actualNotification.TemplateId.Should().Be(createNotificationCommand.MessageDto.TemplateId);
            foreach (var token in createNotificationCommand.MessageDto.TemplateTokens)
            {
                var tokenValue = actualNotification.TokenValues.FirstOrDefault(x => x.Key == token.Key);
                ArgumentNullException.ThrowIfNull(tokenValue);
                tokenValue.Value.Should().Be(token.Value);
            }
        }
    }
}