using MetricsAgent.Responses;
using System.Collections.Generic;

namespace MetricsAgent.Response
{
    public class AllMetricsResponse
    {
        public List<MetricDto> Metrics { get; set; }
    }
}