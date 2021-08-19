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
    public class DotNetMetricsController : ControllerBase
    {
        private DotNetMetricsRepository repository;
        private readonly IMapper mapper;
        public DotNetMetricsController(DotNetMetricsRepository repository, IMapper mapper)
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

        //[HttpGet("timePeriod")]
        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod([FromRoute] TimeSpan fromTime, [FromRoute] TimeSpan toTime)
        {
            var metrics = repository.GetByTimePeriod(fromTime, toTime);

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
