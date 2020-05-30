using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PolygonNET.Network;
using PolygonNET.Utils;

namespace PolygonNET.Tests.Network {
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
        }

        [Test]
        public async Task RequestAsyncCallsCorrectUri() {
            _polygonAuth.Setup(p =>
                                   p.AuthorizeRequest(It.IsAny<string>(),
                                                      It.IsAny<Dictionary<string, string>>(),
                                                      It.IsAny<string>(),
                                                      It.IsAny<string>()));
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

            _ = await _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters, CancellationToken.None);

            var expectedUri = new Uri($"http://example.com/problems.fetch?{parameters.BuildQueryString()}");

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                                                  req.Method == HttpMethod.Post
                                                  && req.RequestUri == expectedUri
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
        public async Task RequestAsyncCallsCorrectUriWithEmptyParams() {
            _polygonAuth.Setup(p =>
                                   p.AuthorizeRequest(It.IsAny<string>(),
                                                      It.IsAny<Dictionary<string, string>>(),
                                                      It.IsAny<string>(),
                                                      It.IsAny<string>()));
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
            _ = await _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, parameters, CancellationToken.None);

            var expectedUri = new Uri($"http://example.com/problems.fetch");

            _httpHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req =>
                                                  req.Method == HttpMethod.Post
                                                  && req.RequestUri == expectedUri
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
        public async Task RequestAsyncReturnsCorrectResponse() {
            const string resContent = @"{
                ""status"": ""OK"",
                ""result"": {
                    ""expectedString"": ""someValue""
                }
            }";
            
            _polygonAuth.Setup(p =>
                                   p.AuthorizeRequest(It.IsAny<string>(),
                                                      It.IsAny<Dictionary<string, string>>(),
                                                      It.IsAny<string>(),
                                                      It.IsAny<string>()));
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

            var response = await _polygonHttpClient.RequestAsync<ExpectedObject>(methodName, new Dictionary<string, string>(),
                                                                      CancellationToken.None);

            Assert.AreEqual("someValue", response.ExpectedString);
        }

        private class ExpectedObject {
            public string ExpectedString { get; set; }
        }
    }
}