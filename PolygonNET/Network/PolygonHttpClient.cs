using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PolygonNET.Network.Exceptions;

namespace PolygonNET.Network {
    /// <summary>
    /// Custom wrapper around an httpClient that takes care of adding auth parameters to a request
    /// and parsing the response.
    /// </summary>
    public interface IPolygonHttpClient {
        /// <summary>
        /// Makes a request to the address with <paramref name="methodName" /> and the specified parameters, returning
        /// a parsed result with type <typeparamref name="T" />.
        /// </summary>
        /// <param name="methodName">Name of the method to be called.</param>
        /// <param name="parameters">Parameters of the call.</param>
        /// <param name="cancellationToken">Cancellation token for the async request.</param>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <returns>The result parsed from the response body.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// This reason can come from the "comment" field from the response body or generic response data
        /// if the body could not be parsed.
        /// </exception>
        public Task<T> RequestAsync<T>(string methodName, Dictionary<string, string> parameters,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Makes a request to the address with <paramref name="methodName" /> and the specified parameters.
        /// </summary>
        /// <param name="methodName">Name of the method to be called.</param>
        /// <param name="parameters">Parameters of the call.</param>
        /// <param name="cancellationToken">Cancellation token for the async request.</param>
        /// <returns>Nothing.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// This reason can come from the "comment" field from the response body or generic response data
        /// if the body could not be parsed.
        /// </exception>
        public Task RequestAsync(string methodName, Dictionary<string, string> parameters,
                                 CancellationToken cancellationToken);

        /// <summary>
        /// Makes a request to the address with <paramref name="methodName" /> and the specified parameters.
        /// </summary>
        /// <param name="methodName">Name of the method to be called.</param>
        /// <param name="parameters">Parameters of the call.</param>
        /// <param name="cancellationToken">Cancellation token for the async request.</param>
        /// <returns>The raw response body.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// This reason can come from the "comment" field from the response body or generic response data
        /// if the body could not be parsed.
        /// </exception>
        public Task<string> RequestContentAsync(string methodName, Dictionary<string, string> parameters,
                                                CancellationToken cancellationToken);
    }

    public class PolygonHttpClient : IPolygonHttpClient {
        private readonly PolygonConfiguration _configuration;
        private readonly IPolygonAuth _auth;
        private readonly HttpClient _httpClient;

        public PolygonHttpClient(IOptions<PolygonConfiguration> options, IPolygonAuth auth) {
            _configuration = options.Value;
            _auth = auth;
            _httpClient = DefaultHttpClient(_configuration.ApiBaseUrl);
        }

        public PolygonHttpClient(IOptions<PolygonConfiguration> options, IPolygonAuth auth,
                                 HttpClient httpClient) {
            _configuration = options.Value;
            _auth = auth;
            _httpClient = httpClient;
        }

        public async Task<T> RequestAsync<T>(string methodName, Dictionary<string, string> parameters,
                                             CancellationToken cancellationToken) {
            var stringContent = await RequestContentAsync(methodName, parameters, cancellationToken);
            var content = JsonConvert.DeserializeObject<PolygonResult<T>>(stringContent);

            if (content.Status == PolygonResponseStatus.Failed) {
                throw new PolygonFailedRequestException(content.Comment);
            }

            return content.Result;
        }

        public async Task RequestAsync(string methodName, Dictionary<string, string> parameters,
                                       CancellationToken cancellationToken) {
            var stringContent = await RequestContentAsync(methodName, parameters, cancellationToken);
            var content = JsonConvert.DeserializeObject<PolygonResponse>(stringContent);

            if (content.Status == PolygonResponseStatus.Failed) {
                throw new PolygonFailedRequestException(content.Comment);
            }
        }

        public async Task<string> RequestContentAsync(string methodName, Dictionary<string, string> parameters,
                                                      CancellationToken cancellationToken) {
            _auth.AuthorizeRequest(methodName, parameters, _configuration.ApiKey, _configuration.ApiSecret);

            var reqContent = new FormUrlEncodedContent(parameters);
            var response = await _httpClient.PostAsync(methodName, reqContent, cancellationToken);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return content;

            try {
                var failedResponse = JsonConvert.DeserializeObject<PolygonResponse>(content);
                throw new PolygonFailedRequestException(failedResponse.Comment);
            }
            catch (JsonException) {
                // in case the request fails for unexpected reasons such as bad gateway (it has happened)
                // this is how we check if the response has an expected format or not
                throw new PolygonFailedRequestException($"{response.ReasonPhrase}: {content}");
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