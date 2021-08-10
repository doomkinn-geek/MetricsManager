using Core;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private NetworkMetricsRepository _repository;
        
        private PerformanceCounter _netCounter;


        public NetworkMetricJob(NetworkMetricsRepository repository)
        {
            _repository = repository;


            PerformanceCounterCategory category = new PerformanceCounterCategory("Network Interface");
            String[] instancename = category.GetInstanceNames();
            _netCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instancename[0]);
        }

        public Task Execute(IJobExecutionContext context)
        {            
            var value = Convert.ToInt32(_netCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DAL.Models.Metric { Time = time, Value = value });

            return Task.CompletedTask;
        }

    }
}
