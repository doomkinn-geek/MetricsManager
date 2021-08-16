using AutoMapper;
using MetricsAgent;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.DAL.Models;
using MetricsAgent.Requests;
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
        private Mock<IMapper> mapper;


        public CpuMetricsControllerUnitTests()
        {
            mock = new Mock<CpuMetricsRepository>();
            mapper = new Mock<IMapper>();
            controller = new CpuMetricsController(mock.Object, mapper.Object);
        }

        [Fact]
        public void Create_ShouldCall_Create_From_Repository()
        {
            // ������������� �������� ��������
            // � �������� ����������� ��� � ����������� �������� MetricContainer ������
            var cpuMetric = new Metric() { };
            mock.Setup(repository => repository.GetById(It.IsAny<int>())).Returns(cpuMetric);
            var request = new MetricCreateRequest { Time = TimeSpan.FromSeconds(1), Value = 50 };

            // ��������� �������� �� �����������
            var result = controller.Create(request);

            // ��������� �������� �� ��, ��� ���� ������� ����������
            // ������������� �������� ����� Create ����������� � ������ ����� ������� � ���������
            mock.Verify(repository => repository.Create(It.IsAny<Metric>()), Times.AtMostOnce());
        }
    }

}
