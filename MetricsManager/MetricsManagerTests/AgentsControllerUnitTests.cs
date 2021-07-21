using MetricsManager;
using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentsControllerUnitTests
    {
        private AgentsController controller;

        public AgentsControllerUnitTests()
        {
            controller = new AgentsController();
        }

        [Fact]
        public void RegisterAgent_ReturnsOk()
        {
            //Arrange
            var agentInfo = new AgentInfo();
            agentInfo.AgentId = 1;
            agentInfo.AgentAddress = new Uri("http://somehost/");            

            //Act
            var result = controller.RegisterAgent(agentInfo);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_ReturnsOk()
        {
            //Arrange            
            var agentId = 1;

            //Act
            var result = controller.EnableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_ReturnsOk()
        {
            //Arrange            
            var agentId = 1;

            //Act
            var result = controller.DisableAgentById(agentId);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void GetAgentsList_ReturnsOk()
        {
            //Arrange            

            //Act
            var result = controller.GetAgentsList();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
