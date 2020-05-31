using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PolygonNET.Utils;

namespace PolygonNET.Tests.Utils {
    [TestFixture]
    public class UtilsServiceCollectionTest {
        private IServiceCollection _sc;

        [OneTimeSetUp]
        public void Setup() {
            _sc = new ServiceCollection();
            _sc.AddUtilsServices();
        }

        [Test]
        public void RandomUtilsIsAddedOnce() {
            var count = _sc.Count(s => s.ServiceType == typeof(IRandomUtils));

            Assert.AreEqual(1, count);
        }

        [Test]
        public void RandomUtilsIsSingleton() {
            var randomUtils = _sc.First(s => s.ServiceType == typeof(IRandomUtils));

            Assert.AreEqual(ServiceLifetime.Singleton, randomUtils.Lifetime);
        }

        [Test]
        public void CryptoUtilsIsAddedOnce() {
            var count = _sc.Count(s => s.ServiceType == typeof(ICryptoUtils));

            Assert.AreEqual(1, count);
        }

        [Test]
        public void CryptoUtilsIsSingleton() {
            var cryptoUtils = _sc.First(s => s.ServiceType == typeof(ICryptoUtils));

            Assert.AreEqual(ServiceLifetime.Singleton, cryptoUtils.Lifetime);
        }
    }
}