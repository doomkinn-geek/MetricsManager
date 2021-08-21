﻿using AutoMapper;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using MetricsManager.Request;
using MetricsManager.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ILogger<AgentsController> _logger;
        private readonly AgentsRepository _repository;
        private readonly IMapper _mapper;
        public AgentsController(ILogger<AgentsController> logger, AgentsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;            
            _mapper = mapper;
            _logger.LogDebug(1, "NLog встроен в AgentsController");
        }
        
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentRequest agentRequest)
        {
            _logger.LogInformation("RegisterAgent: agentInfo = {0}", agentRequest);
            _repository.Create(_mapper.Map<AgentItem>(agentRequest));
            _logger.LogInformation("Agent successfully added to database");
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("EnableAgentById: agentId = {0}", agentId);
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation("DisableAgentById: agentId = {0}", agentId);
            return Ok();
        }

        [HttpGet("list")]
        public IActionResult GetAgentsList()
        {
            var agents = _repository.GetAll();
            var response = new AllAgentsResponse()
            {
                Agents = new List<AgentResponse>()
            };

            foreach (var agent in agents)
            {
                response.Agents.Add(_mapper.Map<AgentResponse>(agent));
            }
            return Ok(response);
        }
    }    
}
