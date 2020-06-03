using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PolygonNET.Network.Exceptions;
using PolygonNET.Utils;

namespace PolygonNET.Network {
    internal class PolygonHttpClient {
        private readonly PolygonConfiguration _configuration;
        private readonly IPolygonAuth _polygonAuth;
        private readonly HttpClient _httpClient;

        public PolygonHttpClient(IOptions<PolygonConfiguration> options, IPolygonAuth polygonAuth,
                                 HttpClient httpClient = null) {
            _configuration = options.Value;
            _polygonAuth = polygonAuth;

            _httpClient = httpClient ?? DefaultHttpClient(_configuration.ApiBaseUrl);
        }

        public async Task<T> RequestAsync<T>(string methodName, Dictionary<string, string> parameters,
                                             CancellationToken cancellationToken) {
            var stringContent = await RequestAsync(methodName, parameters, cancellationToken);
            var content = JsonConvert.DeserializeObject<PolygonResponse<T>>(stringContent);
            
            if (content.Status == PolygonResponseStatus.Failed) {
                throw new FailedRequestException(content.Comment);
            }
            
            return content.Result;
        }

        public async Task<string> RequestAsync(string methodName, Dictionary<string, string> parameters,
                                             CancellationToken cancellationToken) {
            _polygonAuth.AuthorizeRequest(methodName, parameters, _configuration.ApiKey, _configuration.ApiSecret);

            var path = methodName;

            if (parameters.Count > 0) {
                var queryString = parameters.BuildQueryString();
                path = $"{methodName}?{queryString}";
            }
            
            var response = await _httpClient.PostAsync(path, null, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) {
                return content;
            }

            try {
                var failedResponse = JsonConvert.DeserializeObject<PolygonFailedResponse>(content);
                throw new FailedRequestException(failedResponse.Comment);
            } catch (JsonException) {
                throw new FailedRequestException($"{response.ReasonPhrase}: {content}");
            }
        }

        private static HttpClient DefaultHttpClient(string baseUrl) {
            var httpClient = new HttpClient {
                BaseAddress = new Uri(baseUrl),
            };

            httpClient.DefaultRequestHeaders.Accept.ParseAdd(MediaTypeNames.Application.Json);
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Polygon.NET - A .NET client for the Polygon API");

            return httpClient;
        }
    }
}