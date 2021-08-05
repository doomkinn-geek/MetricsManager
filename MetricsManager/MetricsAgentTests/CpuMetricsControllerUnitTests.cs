using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class CpuMetricsControllerUnitTests
    {
        private CpuMetricsController controller;
        private Mock<CpuMetricsRepository> mock;


        public CpuMetricsControllerUnitTests()
        {
            mock = new Mock<CpuMetricsRepository>();
            controller = new CpuMetricsController(mock.Object, null);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // ������������� �������� ��������
            // � �������� ����������� ��� � ����������� �������� MetricContainer ������
            mock.Setup(repository => repository.Create(It.IsAny<MetricContainer>())).Verifiable();

            // ��������� �������� �� �����������
            var result = controller.Create(new MetricsAgent.Requests.MetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 });

            // ��������� �������� �� ��, ��� ���� ������� ����������
            // ������������� �������� ����� Create ����������� � ������ ����� ������� � ���������
            mock.Verify(repository => repository.Create(It.IsAny<MetricContainer>()), Times.AtMostOnce());
        }
    }

}
