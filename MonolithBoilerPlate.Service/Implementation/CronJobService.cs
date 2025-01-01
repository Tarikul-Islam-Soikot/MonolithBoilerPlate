using MonolithBoilerPlate.Service.Helper;
using Microsoft.Extensions.Hosting;

namespace MonolithBoilerPlate.Service.Implementation
{
    public abstract class CronJobService : BackgroundService
    {
        private readonly CronExpressionHelper _cronJobHelper;
        private DateTime _nextRun;
        protected CronJobService(string cronExpression)
        {
            _cronJobHelper = new CronExpressionHelper(cronExpression);
        }

        protected virtual async Task ScheduleJobAsync(CancellationToken stoppingToken)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _nextRun = _cronJobHelper.GetNextOccurrence();
                var delay = (int)(_nextRun - DateTime.Now).TotalMilliseconds;
                await Task.Delay(delay, stoppingToken);
                await ScheduleJobAsync(stoppingToken);
            }
        }

    }
}
