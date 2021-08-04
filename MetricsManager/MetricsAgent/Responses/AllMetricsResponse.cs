using MetricsAgent.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Responses
{
    public class AllMetricsResponse
    {
        public List<MetricContainer> Metrics { get; set; }
    }

}
