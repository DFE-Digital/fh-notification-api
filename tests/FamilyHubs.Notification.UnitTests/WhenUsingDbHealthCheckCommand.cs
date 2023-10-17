using Ardalis.GuardClauses;
using FamilyHubs.Notification.Api.Schedule;
using FamilyHubs.Notification.Core.Queries.DbHealthCheck;
using FamilyHubs.Notification.Data.Interceptors;
using FamilyHubs.Notification.Data.Repository;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;

namespace FamilyHubs.Notification.UnitTests;

public class WhenUsingDbHealthCheckCommand
{
    [Fact]
    public async Task ThenCheckDbConnectionIsSuccessFull()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(new HttpContextAccessor());

        using (var dbContext = new ApplicationDbContext(options, auditableEntitySaveChangesInterceptor))
        {
            // Arrange
            var handler = new DbHealthCheckCommandHandler(dbContext);
            var command = new DbHealthCheckCommand();

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

    }

    [Fact]
    public async Task ThenExecute_CallsMediatorAndLogsResult()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<HealthCheckJob>>();

        var job = new HealthCheckJob(mediatorMock.Object, loggerMock.Object);
        var contextMock = new Mock<IJobExecutionContext>();

        // Set up the Mediator mock to return a specific result.
        mediatorMock.Setup(m => m.Send(It.IsAny<DbHealthCheckCommand>(), CancellationToken.None)).ReturnsAsync(true);

        // Act
        await job.Execute(contextMock.Object);

        // Assert
        mediatorMock.Verify(m => m.Send(It.IsAny<DbHealthCheckCommand>(), CancellationToken.None), Times.Once);
    }
}
