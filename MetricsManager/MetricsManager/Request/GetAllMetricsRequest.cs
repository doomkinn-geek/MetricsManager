using System;

namespace MetricsManager.Request
{
    public class GetAllMetricsRequest
    {
        public string ClientBaseAddress { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}