using AutoMapper;
using MetricsAgent.Client;
using MetricsAgent.Request;
using MetricsAgent.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/cpu")]
    [ApiController]
    public class CpuMetricsController : ControllerBase
    {
        private readonly ILogger<CpuMetricsController> _logger;

        //private CpuMetricsRepository repository;
        //private readonly IMapper mapper;
        private IMetricsAgentClient metricsAgentClient;

        public CpuMetricsController(ILogger<CpuMetricsController> logger, IMetricsAgentClient metrcisAgentClient)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в CpuMetricsController");
            this.metricsAgentClient = metricsAgentClient;
        }


        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("GetMetricsFromAgent: agentId = {0}, fromTime = {1}, toTime = {2}", agentId, fromTime, toTime);            
            var request = new GetAllMetricsRequest
            {
                FromTime = fromTime,
                ToTime = toTime
            };
            var metrics = metricsAgentClient.GetCpuMetrics(request);

            return Ok(metrics);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            _logger.LogInformation("GetMetricsFromAllCluster: fromTime = {1}, toTime = {2}", fromTime, toTime);
            return Ok();
        }
    }

}
