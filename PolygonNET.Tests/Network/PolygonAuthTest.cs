using System;
using System.Collections.Generic;
using Bogus;
using NUnit.Framework;
using PolygonNET.Network;

namespace PolygonNET.Tests.Network {
    public class PolygonAuthTest {
        private Faker _faker;

        [SetUp]
        public void Setup() {
            _faker = new Faker();
        }

        [Test]
        public void AuthorizeRequestCreatesCorrectParams() {
            var methodName = _faker.Internet.UserName();
            var parameters = new Dictionary<string, string> {
                {"key1", "value1"},
                {"key2", "value2"}
            };
            var apiKey = _faker.Random.Guid().ToString();
            var apiSecret = _faker.Random.Guid().ToString();
            var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var authParams = PolygonAuth.AuthorizeRequest(methodName, parameters, apiKey, apiSecret);
            var paramTime = long.Parse(authParams["time"]);

            var diffTime = (time - paramTime);

            Assert.AreEqual(apiKey, authParams["apiKey"]);
            Assert.AreEqual("value1", authParams["key1"]);
            Assert.AreEqual("value2", authParams["key2"]);
            Assert.True(authParams.ContainsKey("apiSig"));
            Assert.LessOrEqual(diffTime, 1000 * 60 * 5);
        }
    }
}