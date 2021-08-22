using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Response
{
    public class MetricDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public int Value { get; set; }
        public int AgentId { get; set; }
    }

}
