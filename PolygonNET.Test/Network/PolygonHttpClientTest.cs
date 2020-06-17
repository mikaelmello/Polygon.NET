using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PolygonNET.Network;
using PolygonNET.Network.Exceptions;

namespace PolygonNET.Test.Network {
    public class PolygonHttpClientTest {
        private Faker _faker;
        private Mock<IPolygonAuth> _polygonAuth;
        private Mock<HttpMessageHandler> _httpHandler;
        private PolygonHttpClient _polygonHttpClient;
        private IOptions<PolygonConfiguration> _options;

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _faker = new Faker();
            _options = Options.Create(new PolygonConfiguration {
                ApiKey = _faker.Random.Hash(),
                ApiSecret = _faker.Random.Hash(),
            });
        }

        [SetUp]
        public void SetUp() {
            _polygonAuth = new Mock<IPolygonAuth>(MockBehavior.Strict);
            _httpHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var httpClient = new HttpClient(_httpHandler.Object) {
                BaseAddress = new Uri("http://example.com/"),
            };

            _polygonHttpClient = new PolygonHttpClient(_options, _polygonAuth.Object, httpClient);

            _polygonAuth.Setup(p =>
                                   p.AuthorizeRequest(It.IsAny<string>(),
                                                      It.IsAny<Dictionary<string, string>>(),
                                                      It.IsAny<string>(),
                                                      It.IsAny<string>()));
        }

        [Test]
        [Description("RequestAsync<T> calls correct uri with correct request body")]
        public async Task RequestAsyncTCallsCorrectUriAndCorrectRequestBody() {
            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"expectedString\": \"someValue\" }"),
                });

            const string methodName = "problems.fetch";

            var parameters = new Dictionary<string, string> {
                {"key1", "value1"},
                {"key2", "value2"},
            };

            _ = await _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters);

            var expectedUri = new Uri("http://example.com/problems.fetch");

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                                                  req.Method == HttpMethod.Post
                                                  && req.RequestUri == expectedUri
                                                  && req.Content.MatchesDictionary(parameters).Result
                ),
                ItExpr.IsAny<CancellationToken>()
            );
            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        [Description("RequestAsync<T> calls correct uri with correct empty request body")]
        public async Task RequestAsyncTCallsCorrectUriWithEmptyParams() {
            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"expectedString\": \"someValue\" }"),
                });

            const string methodName = "problems.fetch";

            var parameters = new Dictionary<string, string>();
            _ = await _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters);

            var expectedUri = new Uri("http://example.com/problems.fetch");

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                                                  req.Method == HttpMethod.Post
                                                  && req.RequestUri == expectedUri
                                                  && req.Content.MatchesDictionary(parameters).Result
                ),
                ItExpr.IsAny<CancellationToken>()
            );
            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task RequestAsyncTReturnsCorrectResponse() {
            const string resContent = @"{
                ""status"": ""OK"",
                ""result"": {
                    ""expectedString"": ""someValue""
                }
            }";

            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(resContent),
                });

            const string methodName = "problems.fetch";

            var response = await _polygonHttpClient
                .RequestAsync<ExpectedObject>(methodName, new Dictionary<string, string>());

            Assert.AreEqual("someValue", response.ExpectedString);
        }

        [Test]
        public void FailedRequestAsyncTWithGenericMessageThrows() {
            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("no healthy upstream"),
                });

            const string methodName = "problems.fetch";
            var parameters = new Dictionary<string, string>();

            var ex = Assert.ThrowsAsync<PolygonFailedRequestException>(
                () => _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters));
            Assert.AreEqual("Internal Server Error: no healthy upstream", ex.Message);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public void FailedRequestAsyncTWithCommentThrows() {
            const string resContent = @"{
                ""status"": ""FAILED"",
                ""comment"": ""failed request, that's too bad""
            }";

            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(resContent),
                });

            const string methodName = "problems.fetch";
            var parameters = new Dictionary<string, string>();

            var ex = Assert.ThrowsAsync<PolygonFailedRequestException>(
                () => _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters));
            Assert.AreEqual("failed request, that's too bad", ex.Message);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public void SuccessfulRequestAsyncTWithFailedContentThrows() {
            const string resContent = @"{
                ""status"": ""FAILED"",
                ""comment"": ""failed request, that's too bad""
            }";

            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(resContent),
                });

            const string methodName = "problems.fetch";
            var parameters = new Dictionary<string, string>();

            var ex = Assert.ThrowsAsync<PolygonFailedRequestException>(
                () => _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters));
            Assert.AreEqual("failed request, that's too bad", ex.Message);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        [Description("RequestContentAsync calls correct uri with correct request body")]
        public async Task RequestContentAsyncCallsCorrectUri() {
            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"expectedString\": \"someValue\" }"),
                });

            const string methodName = "problems.fetch";

            var parameters = new Dictionary<string, string> {
                {"key1", "value1"},
                {"key2", "value2"},
            };

            _ = await _polygonHttpClient.RequestContentAsync(methodName, parameters);

            var expectedUri = new Uri("http://example.com/problems.fetch");

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                                                  req.Method == HttpMethod.Post
                                                  && req.RequestUri == expectedUri
                                                  && req.Content.MatchesDictionary(parameters).Result
                ),
                ItExpr.IsAny<CancellationToken>()
            );
            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        [Description("RequestContentAsync calls correct uri with correct empty request body")]
        public async Task RequestContentAsyncCallsCorrectUriWithEmptyParams() {
            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{ \"expectedString\": \"someValue\" }"),
                });

            const string methodName = "problems.fetch";

            var parameters = new Dictionary<string, string>();
            _ = await _polygonHttpClient.RequestContentAsync(methodName, parameters);

            var expectedUri = new Uri("http://example.com/problems.fetch");

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                                                  req.Method == HttpMethod.Post
                                                  && req.RequestUri == expectedUri
                                                  && req.Content.MatchesDictionary(parameters).Result
                ),
                ItExpr.IsAny<CancellationToken>()
            );
            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task RequestContentAsyncReturnsCorrectResponse() {
            var resContent = _faker.Lorem.Text();

            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(resContent),
                });

            const string methodName = "problems.fetch";

            var response = await _polygonHttpClient
                .RequestContentAsync(methodName, new Dictionary<string, string>());

            Assert.AreEqual(resContent, response);
        }

        [Test]
        public void FailedRequestWithGenericMessageThrows() {
            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("no healthy upstream"),
                });

            const string methodName = "problems.fetch";
            var parameters = new Dictionary<string, string>();

            var ex = Assert.ThrowsAsync<PolygonFailedRequestException>(
                () => _polygonHttpClient.RequestContentAsync(methodName, parameters));
            Assert.AreEqual("Internal Server Error: no healthy upstream", ex.Message);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public void FailedRequestWithCommentParsesAndThrows() {
            const string resContent = @"{
                ""status"": ""FAILED"",
                ""comment"": ""failed request, that's too bad""
            }";

            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(resContent),
                });

            const string methodName = "problems.fetch";
            var parameters = new Dictionary<string, string>();

            var ex = Assert.ThrowsAsync<PolygonFailedRequestException>(
                () => _polygonHttpClient.RequestContentAsync(methodName, parameters));
            Assert.AreEqual("failed request, that's too bad", ex.Message);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        [Test]
        public async Task SuccessfulRequestWithFailedContentReturnsContent() {
            const string resContent = @"{
                ""status"": ""FAILED"",
                ""comment"": ""failed request, that's too bad""
            }";

            _httpHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(resContent),
                });

            const string methodName = "problems.fetch";
            var parameters = new Dictionary<string, string>();

            var response = await _polygonHttpClient.RequestContentAsync(methodName, parameters);
            Assert.AreEqual(resContent, response);

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()
            );
        }

        private class ExpectedObject {
            public string ExpectedString { get; set; }
        }
    }

    public static class TestExtensions {
        public static async Task<bool> MatchesDictionary(this HttpContent httpContent,
                                                         Dictionary<string, string> parameters) {
            var str = await httpContent.ReadAsStringAsync();
            var parsed = HttpUtility.ParseQueryString(str);

            var anyFailureContent = (from key in parsed.AllKeys
                                     let value = parsed[key]
                                     where !parameters.ContainsKey(key) || parameters[key] != value
                                     select key).Any();

            return parameters.Count == parsed.Count && !anyFailureContent;
        }
    }
}