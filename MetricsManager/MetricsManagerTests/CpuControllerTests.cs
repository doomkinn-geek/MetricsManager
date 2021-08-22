using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuControllerTests
    {
        private readonly CpuMetricsController _controller;
        private readonly Mock<CpuMetricsRepository> _moq;

        public CpuControllerTests()
        {
            _moq = new Mock<CpuMetricsRepository>();
            var logMoq = new Mock<ILogger<CpuMetricsController>>();
            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            _controller = new CpuMetricsController(_moq.Object, logMoq.Object, mapper);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(), 
                It.IsAny<DateTimeOffset>(), 
                It.IsAny<int>()))
                .Returns(new List<CpuMetric>()
                {
                    new CpuMetric()
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
            var request = new CpuMetricFromAgentRequests(agentId, fromTime, toTime);

            //Act
            var result = _controller.GetMetricsFromAgent(request);               

            // Assert
            _ = Assert.IsAssignableFrom<CpuGetMetricsFromAgentResponse>(result);
        }

        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            //Arrange
            _moq.Setup(repo => repo.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
                .Returns(new List<CpuMetric>()
                {
                    new CpuMetric()
                    {
                        Time = 1000,
                        Value = 2,
                        Id = 1,
                    }
                }).Verifiable();
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(0);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(100);
            var request = new CpuMetricFromClusterRequests(fromTime, toTime);

            //Act
            var result = _controller.GetMetricsFromAllCluster(request);

            // Assert
            _ = Assert.IsAssignableFrom<CpuGetMetricsFromClusterResponse>(result);
        }
    }
}
