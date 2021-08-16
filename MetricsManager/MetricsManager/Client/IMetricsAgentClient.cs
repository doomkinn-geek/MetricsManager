﻿using MetricsManager.Request;
using MetricsManager.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager.Client
{
    public interface IMetricsAgentClient
    {
        AllMetricsResponse GetAllRamMetrics(GetAllMetricsRequest request);

        AllMetricsResponse GetAllHddMetrics(GetAllMetricsRequest request);

        AllMetricsResponse GetDonNetMetrics(GetAllMetricsRequest request);

        AllMetricsResponse GetCpuMetrics(GetAllMetricsRequest request);

    }

}