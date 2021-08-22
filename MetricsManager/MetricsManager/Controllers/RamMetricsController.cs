using AutoMapper;
using MetricsManager.Client;
using MetricsManager.DAL;
using MetricsManager.Request;
using MetricsManager.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        private readonly ILogger<RamMetricsController> _logger;
        private readonly RamMetricsRepository _repository;
        private readonly IMapper _mapper;
        public RamMetricsController(RamMetricsRepository repository, ILogger<RamMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в RamMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("agent/{agentId}/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] long fromTime, [FromRoute] long toTime)
        {
            _logger.LogInformation("GetMetricsFromAgent: agentId = {0}, fromTime = {1}, toTime = {2}", agentId, fromTime, toTime);
            var queryResult = _repository.GetByTimePeriod(agentId, fromTime, toTime);

            var response = new AllMetricsResponse()
            {
                Metrics = new List<MetricDto>()
            };

            foreach (var metric in queryResult)
            {
                response.Metrics.Add(_mapper.Map<MetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("cluster/from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAllCluster([FromRoute] long fromTime, [FromRoute] long toTime)
        {
            _logger.LogInformation("GetMetricsFromAllCluster: fromTime = {1}, toTime = {2}", fromTime, toTime);
            var queryResult = _repository.GetByTimePeriod(fromTime, toTime);

            var response = new AllMetricsResponse()
            {
                Metrics = new List<MetricDto>()
            };

            foreach (var metric in queryResult)
            {
                response.Metrics.Add(_mapper.Map<MetricDto>(metric));
            }
            return Ok(response);
        }
    }
}
