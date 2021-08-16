using System;

namespace MetricsManager.Request
{
    public class GetAllMetricsRequest
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}