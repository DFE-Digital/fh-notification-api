using FamilyHubs.Notification.Core.Commands.CreateNotification;
using FamilyHubs.Notification.Core.Queries.GetSentNotifications;
using FamilyHubs.Notification.Data.Entities;
using FamilyHubs.Notification.Data.NotificationServices;
using FamilyHubs.Notification.Data.Shared;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyHubs.Notification.UnitTests;

public class WhenGettingSentNotifications : BaseCreateDbUnitTest
{

    [Theory]
    [InlineData(NotificationOrderBy.RecipientEmail, true, 1)]
    [InlineData(NotificationOrderBy.RecipientEmail, false, 2)]
    [InlineData(NotificationOrderBy.Created, true, 1)]
    [InlineData(NotificationOrderBy.Created, false, 2)]
    [InlineData(NotificationOrderBy.TemplateId, true, 1)]
    [InlineData(NotificationOrderBy.TemplateId, false, 2)]
    public async Task ThenGetSentNotifications(NotificationOrderBy orderBy, bool isAssending, int firstId)
    {
        //Arrange
        GetNotificationsCommand command = new GetNotificationsCommand(orderBy, isAssending, 1, 10);
        var context = GetApplicationDbContext();
        context.AddRange(GetNotificationList());
        context.SaveChanges();


        GetNotificationsCommandHandler handler = new GetNotificationsCommandHandler(context, GetMapper());

        //Act
        var result = await handler.Handle(command, new System.Threading.CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items.Count.Should().Be(2);
        result.Items[0].Created.Should().NotBeNull();
        result.Items[0].Id.Should().Be(firstId);
    }

    private List<SentNotification> GetNotificationList()
    {
        return new List<SentNotification>
        {
            new SentNotification
            {
                Id = 1,
                RecipientEmail = "Firstperson@email.com",
                TemplateId = "11111",
                TokenValues = new List<TokenValue>
                {
                    new TokenValue
                    {
                        Id = 1,
                        NotificationId = 1,
                        Key = "Key1",
                        Value = "Value1"
                    }
                }
               
            },

            new SentNotification
            {
                Id = 2,
                RecipientEmail = "Secondperson@email.com",
                TemplateId = "2222",
                TokenValues = new List<TokenValue>
                {
                    new TokenValue
                    {
                        Id = 2,
                        NotificationId = 2,
                        Key = "Key2",
                        Value = "Value2"
                    }
                }

            },
        };
    }
}
