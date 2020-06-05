using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PolygonNET.Network;

namespace PolygonNET.Test.Network {
    [TestFixture]
    public class HttpServiceCollectionTest {
        private IServiceCollection _sc;

        [OneTimeSetUp]
        public void Setup() {
            _sc = new ServiceCollection();
            _sc.AddHttpServices();
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
    }
}
