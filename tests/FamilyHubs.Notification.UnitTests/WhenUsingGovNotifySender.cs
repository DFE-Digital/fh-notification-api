//using FamilyHubs.Notification.Data.NotificationServices;
//using FamilyHubs.Notification.Api.Contracts;
//using FluentAssertions;
//using Microsoft.Extensions.Options;
//using Moq;
//using Notify.Interfaces;

//namespace FamilyHubs.Notification.UnitTests;

//public class WhenUsingGovNotifySender
//{
//    [Fact]
//    public async Task ThenSendConnectNotification()
//    {
//        //Arrange
//        IOptions<GovNotifySetting> mockGovSettings = Options.Create<GovNotifySetting>(new GovNotifySetting
//        {
//            ConnectAPIKey = "ConnectAPIKey",
//            TemplateId = "TemplateId"
//        });

//        Mock<IConnectNotificationClient> mockAsyncNotificationClient = new Mock<IConnectNotificationClient>();
//        int sendEmailCallback = 0;
//        mockAsyncNotificationClient.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(), It.IsAny<string>(), It.IsAny<string>()))
//            .Callback(() => sendEmailCallback++);

//        IEnumerable<IConnectNotificationClient> notificationClients = new List<IConnectNotificationClient>() { mockAsyncNotificationClient.Object };

//        ConnectNotifySender govNotifySender = new ConnectNotifySender(notificationClients, mockGovSettings);
//        var dict = new Dictionary<string, string>();
//        dict.Add("Key1", "Value1");
//        dict.Add("Key2", "Value2");

//        MessageDto messageDto = new MessageDto
//        {
//            ApiKeyType = ApiKeyType.ConnectKey,
//            NotificationEmails = new List<string> { "someone@email.com" },
//            TemplateId = Guid.NewGuid().ToString(),
//            TemplateTokens = dict
//        };

//        //Act
//        await govNotifySender.SendEmailAsync(messageDto);


//        //Assert
//        sendEmailCallback.Should().Be(1);


//    }

//    [Fact]
//    public async Task ThenSendManageNotification()
//    {
//        //Arrange
//        IOptions<GovNotifySetting> mockGovSettings = Options.Create<GovNotifySetting>(new GovNotifySetting
//        {
//            ConnectAPIKey = "ConnectAPIKey",
//            TemplateId = "TemplateId"
//        });

//        Mock<IManageNotificationClient> mockAsyncNotificationClient = new Mock<IManageNotificationClient>();
//        int sendEmailCallback = 0;
//        mockAsyncNotificationClient.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>(), It.IsAny<string>(), It.IsAny<string>()))
//            .Callback(() => sendEmailCallback++);

//        IEnumerable<IManageNotificationClient> notificationClients = new List<IManageNotificationClient>() { mockAsyncNotificationClient.Object };

//        ManageNotifySender govNotifySender = new ManageNotifySender(notificationClients, mockGovSettings);
//        var dict = new Dictionary<string, string>();
//        dict.Add("Key1", "Value1");
//        dict.Add("Key2", "Value2");

//        MessageDto messageDto = new MessageDto
//        {
//            ApiKeyType = ApiKeyType.ConnectKey,
//            NotificationEmails = new List<string> { "someone@email.com" },
//            TemplateId = Guid.NewGuid().ToString(),
//            TemplateTokens = dict
//        };

//        //Act
//        await govNotifySender.SendEmailAsync(messageDto);


//        //Assert
//        sendEmailCallback.Should().Be(1);


//    }
//}