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
    [Route("api/metrics/dotnet")]
    [ApiController]
    public class DotNetMetricsController : ControllerBase
    {
        private readonly ILogger<DotNetMetricsController> _logger;
        private readonly DotNetMetricsRepository _repository;
        private readonly IMapper _mapper;
        public DotNetMetricsController(DotNetMetricsRepository repository, ILogger<DotNetMetricsController> logger, IMapper mapper)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в DotNetMetricsController");
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени по Id агента
        /// </summary>
        /// <param name="agentId">Id агента</param>
        /// <param name="fromTime">Время начала выборки (TimeSpan.Ticks (long)) </param>
        /// <param name="toTime">Время конца выборки (TimeSpan.Ticks (long))</param>
        /// <returns>Список метрик с одного агента</returns>
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

        /// <summary>
        /// Получает метрики DotNet на заданном диапазоне времени со всех агентов
        /// </summary>
        /// <param name="fromTime">Время начала выборки (TimeSpan.Ticks (long)) </param>
        /// <param name="toTime">Время конца выборки (TimeSpan.Ticks (long))</param>
        /// <returns>Список метрик со всех агентов</returns>
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
