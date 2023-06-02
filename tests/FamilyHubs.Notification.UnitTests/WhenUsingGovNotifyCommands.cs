using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Data.NotificationServices;
using FamilyHubs.Notification.Data.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FamilyHubs.Notification.UnitTests;

public class WhenUsingGovNotifyCommands : BaseCreateDbUnitTest
{
    
    [Fact]
    public async Task ThenSendNotificationCommand()
    {
        //Arrange
        var dict = new Dictionary<string, string>();
        dict.Add("Key1", "Value1");
        dict.Add("Key2", "Value2");

        MessageDto messageDto = new MessageDto
        {
            RecipientEmail = "someone@email.com",
            TemplateId = Guid.NewGuid().ToString(),
            TemplateTokens = dict
        };
        CreateNotificationCommand command = new CreateNotificationCommand(messageDto);
        var logger = new Mock<ILogger<CreateNotificationCommandHandler>>();
        Mock<IEmailSender> mockEmailSender = new Mock<IEmailSender>();
        int sendEmailCallback = 0;
        mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<MessageDto>()))
            .Callback(() => sendEmailCallback++);


        CreateNotificationCommandHandler handler = new CreateNotificationCommandHandler(GetApplicationDbContext(), GetMapper(), mockEmailSender.Object, logger.Object);

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().BeTrue();
        sendEmailCallback.Should().Be(1);


    }

    [Fact]
    public async Task ThenSendNotificationCommandThrowsException()
    {
        //Arrange
        var dict = new Dictionary<string, string>();
        dict.Add("Key1", "Value1");
        dict.Add("Key2", "Value2");

        MessageDto messageDto = new MessageDto
        {
            RecipientEmail = "someone@email.com",
            TemplateId = Guid.NewGuid().ToString(),
            TemplateTokens = dict
        };
        CreateNotificationCommand command = new CreateNotificationCommand(messageDto);
        var logger = new Mock<ILogger<CreateNotificationCommandHandler>>();
        Mock<IEmailSender> mockEmailSender = new Mock<IEmailSender>();
        int sendEmailCallback = 0;
        mockEmailSender.Setup(x => x.SendEmailAsync(It.IsAny<MessageDto>()))
            .Callback(() => sendEmailCallback++).Throws(new Exception());

        CreateNotificationCommandHandler handler = new CreateNotificationCommandHandler(GetApplicationDbContext(), GetMapper(), mockEmailSender.Object, logger.Object);

        //Act
        Func<Task> sutMethod = async () => { await handler.Handle(command, new System.Threading.CancellationToken()); };


        //Assert
        await sutMethod.Should().ThrowAsync<Exception>();
        sendEmailCallback.Should().Be(1);
    }
}
