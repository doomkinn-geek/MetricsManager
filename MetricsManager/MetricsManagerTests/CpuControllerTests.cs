using AutoMapper;
using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL;
using MetricsManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuControllerTests
    {
        private readonly CpuMetricsController _controller;
        private readonly Mock<CpuMetricsRepository> _moq;

        public CpuControllerTests()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
            _moq = new Mock<CpuMetricsRepository>(configuration);
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
