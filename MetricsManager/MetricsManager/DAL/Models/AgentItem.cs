using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace MetricsManager.DAL.Models
{
    public class AgentItem
    {
        public int Id { get; set; }
        public Uri AgentUrl { get; set; }
    }
}
