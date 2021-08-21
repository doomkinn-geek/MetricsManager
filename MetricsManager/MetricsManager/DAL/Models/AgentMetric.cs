using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Models
{
    public class AgentMetric
    {
        public int Id { get; set; }
        public int AgentId { get; set; }
        public Uri AgentUrl { get; set; }
    }
}
