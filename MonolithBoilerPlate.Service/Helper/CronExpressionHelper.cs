using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Service.Helper
{
    public class CronExpressionHelper
    {
        private readonly CrontabSchedule _schedule;
        /// <summary>
        /// To generate Cron Expression please follow this link https://crontab.guru
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <param name="includingSeconds"></param>
        public CronExpressionHelper(string cronExpression)
        {
            if (string.IsNullOrEmpty(cronExpression))
                throw new ArgumentNullException(nameof(cronExpression));

            _schedule = CrontabSchedule.Parse(cronExpression,
                  new CrontabSchedule.ParseOptions
                  {
                      IncludingSeconds = true
                  });
        }

        public DateTime GetNextOccurrence()
        {
            var nextRun = _schedule.GetNextOccurrence(DateTime.Now);
            return nextRun;
        }

        public IEnumerable<DateTime> GetNextOccurrences(DateTime baseTime, DateTime endTime)
        {
            var nextRuns = _schedule.GetNextOccurrences(baseTime, endTime);
            return nextRuns;
        }
    }
}
