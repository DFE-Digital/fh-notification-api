using FamilyHubs.Notification.Api.Contracts;
using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FluentAssertions;

namespace FamilyHubs.Notification.UnitTests;

public class WhenValidatingNotificationCommands
{
    [Fact]
    public void ThenShouldNotErrorWhenCreateCommandCommandModelIsValid()
    {
        //Arrange
        var validator = new CreateNotificationCommandValidator();
        var testModel = new CreateNotificationCommand(new MessageDto
        {
            ApiKeyType = ApiKeyType.ConnectKey,
            NotificationEmails = new List<string> { "someone@email.com "},
            TemplateId = "12e7463d-dbb5-4beb-a321-3ea5b55bb642",
            TemplateTokens = new Dictionary<string, string>
            {
                {  "Key1", "Value1" }
            }
        });

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenCreateCommandCommandModelHasNoRecipientEmail()
    {
        //Arrange
        var validator = new CreateNotificationCommandValidator();
        var testModel = new CreateNotificationCommand(new MessageDto
        {
            ApiKeyType = ApiKeyType.ConnectKey,
            NotificationEmails = default!,
            TemplateId = "12e7463d-dbb5-4beb-a321-3ea5b55bb642",
            TemplateTokens = new Dictionary<string, string>
            {
                {  "Key1", "Value1" }
            }
        });

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }

    [Fact]
    public void ThenShouldErrorWhenCreateCommandCommandModelHasNoTemplateId()
    {
        //Arrange
        var validator = new CreateNotificationCommandValidator();
        var testModel = new CreateNotificationCommand(new MessageDto
        {
            ApiKeyType = ApiKeyType.ConnectKey,
            NotificationEmails = new List<string> { "someone@email.com " },
            TemplateId = string.Empty,
            TemplateTokens = new Dictionary<string, string>
            {
                {  "Key1", "Value1" }
            }
        });

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeTrue();
    }
}
