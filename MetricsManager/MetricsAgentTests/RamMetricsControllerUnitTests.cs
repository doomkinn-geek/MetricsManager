using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class RamMetricsControllerUnitTests
    {
        private RamMetricsController controller;
        private Mock<RamMetricsRepository> mock;

        public RamMetricsControllerUnitTests()
        {
            mock = new Mock<RamMetricsRepository>();
            controller = new RamMetricsController(mock.Object);
        }

        [Fact]
        public void GetByTimePeriod_ReturnsOk()
        {
            //Arrange
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = controller.GetByTimePeriod(fromTime, toTime);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetAll_ReturnsOk()
        {
            //Arrange            

            //Act
            var result = controller.GetAll();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
