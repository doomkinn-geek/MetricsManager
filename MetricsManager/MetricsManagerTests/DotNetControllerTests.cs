using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.Controllers.Requests;
using MetricsManager.Controllers.Responses;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class DotNetControllerTests
    {
        private readonly DotNetMetricsController _controller;
        private readonly Mock<IDotNetMetricsRepository> _moq;

        public DotNetControllerTests()
        {
            _moq = new Mock<IDotNetMetricsRepository>();
            var logMoq = new Mock<ILogger<DotNetMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            _controller = new DotNetMetricsController(_moq.Object, logMoq.Object, mapper);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<int>()))
                .Returns(new List<DotNetMetric>()
                {
                    new DotNetMetric()
                    {
                        Time = 1000,
                        Value = 2,
                        Id = 1,
                        AgentId = 1
                    }
                }).Verifiable();
            var agentId = 1;
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);
            var request = new DotNetMetricFromAgentRequests(agentId, fromTime, toTime);

            //Act
            var result = _controller.GetMetricsFromAgent(request);

            // Assert
            _ = Assert.IsAssignableFrom<DotNetGetMetricsFromAgentResponse>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
                .Returns(new List<DotNetMetric>()
                {
                    new DotNetMetric()
                    {
                        Time = 1000,
                        Value = 2,
                        Id = 1,
                    }
                }).Verifiable();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);
            var request = new DotNetMetricFromClusterRequests(fromTime, toTime);

            //Act
            var result = _controller.GetMetricsFromAllCluster(request);

            // Assert
            _ = Assert.IsAssignableFrom<DotNetGetMetricsFromClusterResponse>(result);
        }
    }
}
