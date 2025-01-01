using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonolithBoilerPlate.Common.Interface
{
    public interface ICronConfig<T>
    {/// <summary>
     /// https://crontab.guru/#*/1_*_*_*_*
     /// </summary>
         string CronExpression { get; set; }
    }
}
