using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class HddControllerTests
    {
        private readonly HddMetricsController _controller;
        private readonly Mock<HddMetricsRepository> _moq;

        public HddControllerTests()
        {
            _moq = new Mock<HddMetricsRepository>();
            var logMoq = new Mock<ILogger<HddMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            _controller = new HddMetricsController(_moq.Object, logMoq.Object, mapper);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetByTimePeriod(
                It.IsAny<int>(),
                It.IsAny<long>(),
                It.IsAny<long>()))
                .Returns(new List<Metric>()
                {
                    new Metric()
                    {
                        Time = new TimeSpan(1000),
                        Value = 2,
                        Id = 1,
                        AgentId = 1
                    }
                }).Verifiable();
            var agentId = 1;
            var fromTime = 0;
            var toTime = 100;
            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);
            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetByTimePeriod(
                It.IsAny<long>(),
                It.IsAny<long>()))
                .Returns(new List<Metric>()
                {
                    new Metric()
                    {
                        Time = new TimeSpan(1000),
                        Value = 2,
                        Id = 1,
                    }
                }).Verifiable();
            var fromTime = 0;
            var toTime = 100;

            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
