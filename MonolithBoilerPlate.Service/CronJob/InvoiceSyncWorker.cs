using MonolithBoilerPlate.Common.Interface;
using MonolithBoilerPlate.Service.Implementation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Service.CronJob
{
    public class InvoiceSyncWorker : CronJobService
    {
        private readonly ILogger<InvoiceSyncWorker> _logger;
        public InvoiceSyncWorker(
            ICronConfig<InvoiceSyncWorker> config,
            ILogger<InvoiceSyncWorker> logger) : base(config.CronExpression)
        {
            _logger = logger;
        }

        protected override async Task ScheduleJobAsync(CancellationToken stoppingToken)
        {
            var executeTime = DateTime.Now;
            var workerName = "InvoiceSyncWorker";
            _logger.LogInformation("{ExecuteTime}: {WorkerName} - Started", executeTime, workerName);
            try
            {
                _logger.LogInformation("{ExecuteTime}: {WorkerName} executed", executeTime, workerName);
            }
            catch (Exception exception)
            {
                _logger.LogError("{ExecuteTime}: {WorkerName}. Error: {Exception}", executeTime, workerName, exception.ToString());
            }
            finally
            {
                _logger.LogInformation("{ExecuteTime}: {WorkerName} ended.", executeTime, workerName);
            }
        }
    }
}
