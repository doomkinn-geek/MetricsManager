using MetricsManager.Request;
using MetricsManager.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MetricsManager.Client
{
    public class MetricsAgentClient : IMetricsAgentClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public AllMetricsResponse GetAllHddMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"api/hddmetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public AllMetricsResponse GetAllRamMetrics(GetAllMetricsRequest request)
        {
            throw new NotImplementedException();
        }

        public AllMetricsResponse GetCpuMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"api/cpumetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                return JsonSerializer.DeserializeAsync<AllMetricsResponse>(responseStream).Result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public AllMetricsResponse GetDonNetMetrics(GetAllMetricsRequest request)
        {
            throw new NotImplementedException();
        }
    }

}
