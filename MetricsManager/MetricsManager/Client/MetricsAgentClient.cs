using MetricsManager.Request;
using MetricsManager.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            //this.httpClient = httpClient;
            this.httpClient = new HttpClient(clientHandler);
            this.logger = logger;
        }

        public AllMetricsResponse GetAllHddMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.Ticks;
            var toParameter = request.ToTime.Ticks;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, 
                    $"{request.ClientBaseAddress}api/hddmetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<AllMetricsResponse>(content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public AllMetricsResponse GetAllRamMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.Ticks;
            var toParameter = request.ToTime.Ticks;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"{request.ClientBaseAddress}api/rammetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<AllMetricsResponse>(content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public AllMetricsResponse GetCpuMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.Ticks;
            var toParameter = request.ToTime.Ticks;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, 
                    $"{request.ClientBaseAddress}api/cpumetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<AllMetricsResponse>(content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public AllMetricsResponse GetDotNetMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.Ticks;
            var toParameter = request.ToTime.Ticks;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"{request.ClientBaseAddress}api/dotnetmetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<AllMetricsResponse>(content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }

        public AllMetricsResponse GetNetworkMetrics(GetAllMetricsRequest request)
        {
            var fromParameter = request.FromTime.Ticks;
            var toParameter = request.ToTime.Ticks;
            var httpRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"{request.ClientBaseAddress}api/networkmetrics/from/{fromParameter}/to/{toParameter}");
            try
            {
                HttpResponseMessage response = httpClient.SendAsync(httpRequest).Result;

                using var responseStream = response.Content.ReadAsStreamAsync().Result;

                using var streamReader = new StreamReader(responseStream);
                var content = streamReader.ReadToEnd();
                var result = JsonSerializer.Deserialize<AllMetricsResponse>(content, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return null;
        }
    }

}
