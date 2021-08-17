using MetricsAgent.Request;
using MetricsAgent.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Client
{
    public interface IMetricsAgentClient
    {
        AllMetricsResponse GetAllRamMetrics(GetAllMetricsRequest request);

        AllMetricsResponse GetAllHddMetrics(GetAllMetricsRequest request);

        AllMetricsResponse GetDonNetMetrics(GetAllMetricsRequest request);

        AllMetricsResponse GetCpuMetrics(GetAllMetricsRequest request);

    }

}
