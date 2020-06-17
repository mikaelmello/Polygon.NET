using System.Linq;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using PolygonNET.Network;

namespace PolygonNET.Test {
    [TestFixture]
    public class PolygonServiceCollectionTest {
        private Faker _faker;
        private IServiceCollection _sc;
        private PolygonConfiguration _configuration;

        [OneTimeSetUp]
        public void Setup() {
            _faker = new Faker();
            _sc = new ServiceCollection();

            _configuration = new PolygonConfiguration {
                ApiKey = _faker.Random.Hash(),
                ApiSecret = _faker.Random.Hash(),
                ApiBaseUrl = _faker.Internet.Url(),
            };

            _sc.AddPolygonClient(option => {
                option.ApiKey = _configuration.ApiKey;
                option.ApiSecret = _configuration.ApiSecret;
                option.ApiBaseUrl = _configuration.ApiBaseUrl;
            });
        }

        [Test]
        public void PolygonConfigurationIsConfigured() {
            static bool Check(ServiceDescriptor s) {
                return s.ServiceType == typeof(IConfigureOptions<PolygonConfiguration>);
            }

            var count = _sc.Count(Check);

            Assert.AreEqual(1, count);

            var config = _sc.BuildServiceProvider().GetService<IOptions<PolygonConfiguration>>().Value;
            Assert.AreEqual(_configuration.ApiKey, config.ApiKey);
            Assert.AreEqual(_configuration.ApiSecret, config.ApiSecret);
            Assert.AreEqual(_configuration.ApiBaseUrl, config.ApiBaseUrl);
        }

        [Test]
        public void PolygonAuthIsAddedOnce() {
            var count = _sc.Count(s => s.ServiceType == typeof(IPolygonAuth));

            Assert.AreEqual(1, count);
        }

        [Test]
        public void PolygonAuthIsSingleton() {
            var polygonAuth = _sc.First(s => s.ServiceType == typeof(IPolygonAuth));

            Assert.AreEqual(ServiceLifetime.Singleton, polygonAuth.Lifetime);
        }

        [Test]
        public void PolygonHttpClientIsAddedOnce() {
            var count = _sc.Count(s => s.ServiceType == typeof(IPolygonHttpClient));

            Assert.AreEqual(1, count);
        }

        [Test]
        public void PolygonHttpClientIsSingleton() {
            var polygonHttpClient = _sc.First(s => s.ServiceType == typeof(IPolygonHttpClient));

            Assert.AreEqual(ServiceLifetime.Singleton, polygonHttpClient.Lifetime);
        }

        [Test]
        public void PolygonClientIsAddedOnce() {
            var count = _sc.Count(s => s.ServiceType == typeof(PolygonClient));

            Assert.AreEqual(1, count);
        }

        [Test]
        public void PolygonClientIsSingleton() {
            var polygonClient = _sc.First(s => s.ServiceType == typeof(PolygonClient));

            Assert.AreEqual(ServiceLifetime.Singleton, polygonClient.Lifetime);
        }
    }
}