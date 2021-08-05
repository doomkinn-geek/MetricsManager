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
    public class HddMetricJob : IJob
    {
        private HddMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _hddCounter;


        public HddMetricJob(HddMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("PhysicalDisk", "Available MBytes");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_hddCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DAL.Models.MetricContainer { Time = time, Value = cpuUsageInPercents });

            return Task.CompletedTask;
        }

    }
}
