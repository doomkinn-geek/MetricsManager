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
        private readonly ILogger<MetricsAgentClient> logger;

        public MetricsAgentClient(HttpClient httpClient, ILogger<MetricsAgentClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public AllMetricsResponse GetAllHddMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, 
                    $"{request.ClientBaseAddress}/api/hddmetrics/from/{fromParameter}/to/{toParameter}");
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
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"{request.ClientBaseAddress}/api/rammetrics/from/{fromParameter}/to/{toParameter}");
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

        public AllMetricsResponse GetCpuMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, 
                    $"{request.ClientBaseAddress}/api/cpumetrics/from/{fromParameter}/to/{toParameter}");
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
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"{request.ClientBaseAddress}/api/dotnetmetrics/from/{fromParameter}/to/{toParameter}");
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

        public AllMetricsResponse GetNetworkMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.TotalSeconds;
            var toParameter = request.ToTime.TotalSeconds;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"{request.ClientBaseAddress}/api/networkmetrics/from/{fromParameter}/to/{toParameter}");
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
    }

}
