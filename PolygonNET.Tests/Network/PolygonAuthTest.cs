using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Bogus;
using Moq;
using NUnit.Framework;
using PolygonNET.Network;
using PolygonNET.Utils;

namespace PolygonNET.Tests.Network {
    [TestFixture]
    public class PolygonAuthTest {
        private Mock<IRandomUtils> _randomUtils;
        private Mock<ICryptoUtils> _cryptoUtils;
        private IPolygonAuth _polygonAuth;
        private Faker _faker;

        [OneTimeSetUp]
        public void Setup() {
            _faker = new Faker();
        }

        [SetUp]
        public void BeforeEach() {
            _randomUtils = new Mock<IRandomUtils> {CallBase = true};
            _cryptoUtils = new Mock<ICryptoUtils> {CallBase = true};
            _polygonAuth = new PolygonAuth(_randomUtils.Object, _cryptoUtils.Object);
        }

        [Test]
        public void AuthorizeRequestCreatesCorrectParams() {
            var methodName = _faker.Internet.UserName();
            var apiKey = _faker.Random.Guid().ToString();
            var apiSecret = _faker.Random.Guid().ToString();
            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            var expectedSignature = _faker.Random.Hexadecimal(128);
            _cryptoUtils.Setup(c => c.ComputeSha512Hash(It.IsAny<string>()))
                        .Returns(expectedSignature);

            var parameters = new Dictionary<string, string> {
                {"key1", "value1"},
                {"key2", "value2"}
            };
            
            _polygonAuth.AuthorizeRequest(methodName, parameters, apiKey, apiSecret);
            
            var paramTime = long.Parse(parameters["time"]);
            var diffTime = (time - paramTime);
            Assert.LessOrEqual(diffTime, 1000 * 60 * 5);
            
            Assert.AreEqual(apiKey, parameters["apiKey"]);
            Assert.AreEqual("value1", parameters["key1"]);
            Assert.AreEqual("value2", parameters["key2"]);
            Assert.AreEqual(expectedSignature, parameters["apiSig"]);
        }

        [Test]
        public void GetApiSignatureCreatesCorrectHash() {
            _randomUtils.Setup(r => r.GetRandomAlphanumericString(It.IsAny<int>()))
                        .Returns("qwerty");
            _cryptoUtils.Setup(c => c.ComputeSha512Hash(It.IsAny<string>()))
                        .Returns<string>(Sha512);

            var apiSecret = _faker.Random.Hexadecimal(128);
            var methodName = _faker.Random.String2(12);
            var parameters = new Dictionary<string, string> {
                {"key1", "value1"},
                {"key2", "value2"},
                {"abc", "def"},
            };
            var expected = Sha512($"qwerty/{methodName}?abc=def&key1=value1&key2=value2#{apiSecret}");

            var apiSignature = _polygonAuth.GetApiSignature(methodName, parameters, apiSecret);

            _randomUtils.Verify(r => r.GetRandomAlphanumericString(It.IsAny<int>()), Times.Once);
            _cryptoUtils.Verify(c => c.ComputeSha512Hash(It.IsAny<string>()), Times.Once);

            Assert.AreEqual(expected, apiSignature);
        }

        [Test]
        public void GetApiSignatureUsesCorrectRandLen() {
            const int expectedRandLen = 6;
            
            _ = _polygonAuth.GetApiSignature(default, new Dictionary<string, string>(), default);

            _randomUtils.Verify(r => r.GetRandomAlphanumericString(It.IsAny<int>()), Times.Once);
            _randomUtils.Verify(r => r.GetRandomAlphanumericString(expectedRandLen), Times.Once);
        }

        private static string Sha512(string plainText) {
            using var sha512 = new SHA512Managed();
            var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}