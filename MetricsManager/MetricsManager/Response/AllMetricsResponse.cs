using MetricsManager.Responses;
using System.Collections.Generic;

namespace MetricsManager.Response
{
    public class AllMetricsResponse
    {
        public List<MetricDto> Metrics { get; set; }
    }
}