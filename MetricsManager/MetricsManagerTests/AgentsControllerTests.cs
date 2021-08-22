using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL.Models;
using MetricsManager.DAL.Repositories;
using MetricsManager.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerTests
    {
        private readonly AgentsController _controller;
        private readonly Mock<AgentsRepository> _moq;

        public AgentsControllerTests()
        {
            _moq = new Mock<AgentsRepository>();
            var logMoq = new Mock<ILogger<AgentsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            _controller = new AgentsController(logMoq.Object, _moq.Object, mapper);
        }

        [Fact]
        public void RegisterAgent_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetAll()).Returns(new List<AgentItem>()).Verifiable();
            //Act
            var result = _controller.RegisterAgent(new AgentRequest());
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            //Arrange
            var agentId = 1;

            //Act
            var result = _controller.EnableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void DisableAgentById_ReturnsOk()
        {
            //Arrange
            var agentId = 1;

            //Act
            var result = _controller.DisableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
        [Fact]
        public void GetRegisteredAgents_ReturnsEmptyEnumerable()
        {
            //Arrange
            _moq.Setup(repo => repo.GetAll()).Returns(new List<AgentItem>()).Verifiable();

            //Act
            var result = _controller.GetAgentsList();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
