using System;

namespace MetricsAgent.Request
{
    public class GetAllMetricsRequest
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}