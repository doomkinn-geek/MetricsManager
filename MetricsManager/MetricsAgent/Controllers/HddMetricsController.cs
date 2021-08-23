using AutoMapper;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using MetricsAgent.Requests;
using MetricsAgent.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace MetricsAgent.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        private HddMetricsRepository repository;
        private readonly IMapper mapper;
        public HddMetricsController(HddMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] MetricCreateRequest request)
        {
            repository.Create(new Metric
            {
                Time = request.Time,
                Value = request.Value
            });

            return Ok();
        }
        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int id)
        {
            repository.Delete(id);
            return Ok();
        }
        [HttpPut("update")]
        public IActionResult Update([FromBody] Metric request)
        {
            repository.Update(new Metric
            {
                Id = request.Id,
                Time = request.Time,
                Value = request.Value
            });
            return Ok();
        }

        /// <summary>
        /// Получает метрики HDD записанные за все время наблюдений
        /// </summary>        
        /// <returns>Список метрик</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">Eсли передали не правильные параметры</response>
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var metrics = repository.GetAll();

            var response = new AllMetricsResponse()
            {
                Metrics = new List<MetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<MetricDto>(metric));
            }

            return Ok(response);
        }

        [HttpGet("byId")]
        public IActionResult GetById([FromQuery] int id)
        {
            return Ok(repository.GetById(id));
        }

        /// <summary>
        /// Получает метрики HDD на заданном диапазоне времени
        /// </summary>
        /// <param name="fromTime">TimeSpan начала выборки (тип long)</param>
        /// <param name="toTime">TimeSpan конца выборки (тип long)</param>
        /// <returns>Список метрик</returns>
        /// <response code="201">Если все хорошо</response>
        /// <response code="400">Eсли передали не правильные параметры</response>
        [Route("from/{fromTime}/to/{toTime}")]
        [HttpGet]
        public IActionResult GetByTimePeriod(long fromTime, long toTime)
        {
            TimeSpan fromTimeTS = new TimeSpan(fromTime);
            TimeSpan toTimeTS = new TimeSpan(toTime);
            var metrics = repository.GetByTimePeriod(fromTimeTS, toTimeTS);

            var response = new AllMetricsResponse()
            {
                Metrics = new List<MetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(mapper.Map<MetricDto>(metric));
            }

            return Ok(response);
        }
    }
}
