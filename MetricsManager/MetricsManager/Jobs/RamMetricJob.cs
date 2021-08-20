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
    public class RamMetricJob : IJob
    {
        private AgentsRepository _agentsRepository;
        private RamMetricsRepository _repository;
        private IMetricsAgentClient _client;
        private readonly IMapper _mapper;
        public RamMetricJob(RamMetricsRepository repository, AgentsRepository agents, IMetricsAgentClient client, IMapper mapper)
        {
            _repository = repository;
            _agentsRepository = agents;
            _client = client;
            _mapper = mapper;
        }

        public Task Execute(IJobExecutionContext context)
        {
            IList<AgentMetric> agentsList = _agentsRepository.GetAll();
            foreach (AgentMetric agent in agentsList)
            {
                var request = new GetAllMetricsRequest { ClientBaseAddress = agent.AgentUrl.ToString(), FromTime = _repository.GetMaxRegisteredDate(), ToTime = DateTime.UtcNow.TimeOfDay };
                var response = new AllMetricsResponse()
                {
                    Metrics = new List<MetricDto>()
                };
                response = _client.GetAllRamMetrics(request);

                foreach (var metric in response.Metrics)
                {
                    _repository.Create(_mapper.Map<Metric>(metric));
                }
            }

            return Task.CompletedTask;
        }

    }
}
