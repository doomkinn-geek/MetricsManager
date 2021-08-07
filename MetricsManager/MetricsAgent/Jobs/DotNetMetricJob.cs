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
    public class DotNetMetricJob : IJob
    {
        private DotNetMetricsRepository _repository;
        // счетчик для метрики CPU
        private PerformanceCounter _dotNetCounter;


        public DotNetMetricJob(DotNetMetricsRepository repository)
        {
            _repository = repository;
            //_dotNetCounter = new PerformanceCounter("LogicalDisk", "Free Megabytes", "_Total");???          
        }

        public Task Execute(IJobExecutionContext context)
        {
            //var cpuUsageInPercents = Convert.ToInt32(_dotNetCounter.NextValue());
            var value = GC.GetTotalMemory(true);//может быть так можно получить размер кучи

            // узнаем когда мы сняли значение метрики.
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            // теперь можно записать что-то при помощи репозитория

            _repository.Create(new DAL.Models.MetricContainer { Time = time, Value = (int)value });

            return Task.CompletedTask;
        }

    }
}
