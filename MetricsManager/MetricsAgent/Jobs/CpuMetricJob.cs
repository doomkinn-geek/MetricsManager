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
    public class CpuMetricJob : IJob
    {
        private CpuMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _cpuCounter;


        public CpuMetricJob(CpuMetricsRepository repository)
        {
            _repository = repository;
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        public Task Execute(IJobExecutionContext context)
        {
            // получаем значение занятости CPU
            var cpuUsageInPercents = Convert.ToInt32(_cpuCounter.NextValue());

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DAL.Models.MetricContainer { Time = time, Value = cpuUsageInPercents });

            return Task.CompletedTask;
        }
    }
}


