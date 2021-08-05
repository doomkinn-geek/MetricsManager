﻿using AutoMapper;
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
    public class RamMetricsController : ControllerBase
    {
        private RamMetricsRepository repository;
        private readonly IMapper mapper;
        public RamMetricsController(RamMetricsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] MetricCreateRequest request)
        {
            repository.Create(new MetricContainer
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
        public IActionResult Update([FromBody] MetricContainer request)
        {
            repository.Update(new MetricContainer
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

        [HttpGet("timePeriod")]
        public IActionResult GetByTimePeriod([FromQuery] TimeSpan fromTime, TimeSpan toTime)
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
