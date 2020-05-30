using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PolygonNET.Network.Exceptions;
using PolygonNET.Utils;

namespace PolygonNET.Network {
    public class PolygonHttpClient {
        private const string BaseUrl = "https://polygon.codeforces.com/api/";

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiSecret;
        private readonly string _customBaseUrl;

        public PolygonHttpClient(string apiKey, string apiSecret, string customBaseUrl = null, HttpClient httpClient = null) {
            _httpClient = httpClient ?? new HttpClient();
            _apiKey = apiKey;
            _apiSecret = apiSecret;
            _customBaseUrl = customBaseUrl;

            ConfigureHttpClient();
        }

        public async Task<T> RequestAsync<T>(string methodName, Dictionary<string, string> parameters,
                                             CancellationToken cancellationToken) {
            parameters = PolygonAuth.AuthorizeRequest(methodName, parameters, _apiKey, _apiSecret);

            var uri = new UriBuilder(methodName) {Query = parameters.BuildQueryString()};
            var response = await _httpClient.PostAsync(uri.Uri, null, cancellationToken);
            var contentStream = await response.Content.ReadAsStringAsync();

            var content = JsonConvert.DeserializeObject<PolygonResponse<T>>(contentStream);
            if (!response.IsSuccessStatusCode || content.Status == PolygonResponseStatus.Failed) {
                throw new FailedRequestException(content.Comment);
            }

            return content.Result;
        }

        private void ConfigureHttpClient() {
            _httpClient.BaseAddress = new Uri(_customBaseUrl ?? BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd(MediaTypeNames.Application.Json);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Polygon.NET - A .NET client for the Polygon API");
        }
    }
}