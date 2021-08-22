using AutoMapper;
using Core;
using MetricsManager.Client;
using MetricsManager.DAL;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using MetricsManager.Request;
using MetricsManager.Response;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Jobs
{
    public class CpuMetricJob : IJob
    {
        private AgentsRepository _agentsRepository;
        private CpuMetricsRepository _repository;
        private IMetricsAgentClient _client;
        private readonly IMapper _mapper;
        public CpuMetricJob(CpuMetricsRepository repository, AgentsRepository agents, IMetricsAgentClient client, IMapper mapper)
        {
            _repository = repository;
            _agentsRepository = agents;
            _client = client;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            IList<AgentItem> agentsList = _agentsRepository.GetAll();            
            foreach (AgentItem agent in agentsList)
            {
                var request = new GetAllMetricsRequest { ClientBaseAddress = agent.AgentUrl.ToString(), 
                    FromTime = _repository.GetMaxRegisteredDate(agent.Id), 
                    ToTime = DateTime.UtcNow.TimeOfDay };                                
                var response = new AllMetricsResponse()
                {
                    Metrics = new List<MetricDto>()
                };
                response = _client.GetCpuMetrics(request);
                if (response == null) return Task.CompletedTask;
                if (response.Metrics == null) return Task.CompletedTask;

                foreach (var metric in response.Metrics)
                {
                    _repository.Create(new Metric
                    {
                        AgentId = agent.Id,
                        Time = metric.Time.TimeOfDay,
                        Value = metric.Value
                    });
                }
            }
            
            return Task.CompletedTask;
        }
    }
}


