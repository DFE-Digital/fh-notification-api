using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Data.NotificationServices;
using FamilyHubs.Notification.Api.Contracts;
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
        var dict = new Dictionary<string, string>
        {
            {"Key1", "Value1"},
            {"Key2", "Value2"}
        };

        MessageDto messageDto = new MessageDto
        {
            ApiKeyType = ApiKeyType.ConnectKey,
            NotificationEmails = new List<string> { "someone@email.com" },
            TemplateId = Guid.NewGuid().ToString(),
            TemplateTokens = dict
        };
        CreateNotificationCommand command = new CreateNotificationCommand(messageDto);
        var logger = new Mock<ILogger<CreateNotificationCommandHandler>>();
        Mock<IGovNotifySender> govNotifySender = new Mock<IGovNotifySender>();
        int sendEmailCallback = 0;
        govNotifySender.Setup(x => x.SendEmailAsync(It.IsAny<MessageDto>()))
            .Callback(() => sendEmailCallback++);

        var handler = new CreateNotificationCommandHandler(GetApplicationDbContext(), govNotifySender.Object, GetMapper(), logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().BeTrue();
        sendEmailCallback.Should().Be(1);
    }

    [Fact]
    public async Task ThenSendNotificationCommandThrowsException()
    {
        //Arrange
        var dict = new Dictionary<string, string>
        {
            {"Key1", "Value1"},
            {"Key2", "Value2"}
        };

        MessageDto messageDto = new MessageDto
        {
            ApiKeyType = ApiKeyType.ConnectKey,
            NotificationEmails = new List<string> { "someone@email.com" },
            TemplateId = Guid.NewGuid().ToString(),
            TemplateTokens = dict
        };
        CreateNotificationCommand command = new CreateNotificationCommand(messageDto);
        var logger = new Mock<ILogger<CreateNotificationCommandHandler>>();
        Mock<IGovNotifySender> govNotifySender = new Mock<IGovNotifySender>();
        int sendEmailCallback = 0;
        govNotifySender.Setup(x => x.SendEmailAsync(It.IsAny<MessageDto>()))
            .Callback(() => sendEmailCallback++).Throws(new Exception());

        var handler = new CreateNotificationCommandHandler(GetApplicationDbContext(), govNotifySender.Object, GetMapper(), logger.Object);

        //Act
        Func<Task> sutMethod = async () => { await handler.Handle(command, new CancellationToken()); };

        //Assert
        await sutMethod.Should().ThrowAsync<Exception>();
        sendEmailCallback.Should().Be(1);
    }
}
