using MonolithBoilerPlate.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Common.Models
{
    public class CronConfig<T> : ICronConfig<T>
    {
        /// <summary>
        /// https://crontab.guru/#*/1_*_*_*_*
        /// </summary>
        public string CronExpression { get; set; }
    }
}
