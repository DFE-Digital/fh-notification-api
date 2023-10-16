using FamilyHubs.Notification.Core.Queries.DbHealthCheck;
using MediatR;
using Quartz;
using System.Threading;

namespace FamilyHubs.Notification.Api.Schedule
{
    public class HealthCheckJob : IJob
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HealthCheckJob> _logger;
        public HealthCheckJob(IMediator mediator, ILogger<HealthCheckJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            DbHealthCheckCommand command = new();
            var result = await _mediator.Send(command, CancellationToken.None);
            _logger.LogInformation($"Scheduler Health Check Result = {result}");
        }
    }
}
