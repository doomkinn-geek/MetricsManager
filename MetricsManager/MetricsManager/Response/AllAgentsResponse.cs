using MetricsManager.DAL.Models;
using System.Collections.Generic;

namespace MetricsManager.Response
{
    public class AllAgentsResponse
    {
        public List<AgentResponse> Agents { get; set; }
    }
}
