using System.Collections.Generic;
using System.Web;
using Bogus;
using NUnit.Framework;
using PolygonNET.Utils;

namespace PolygonNET.Test.Utils {
    [TestFixture]
    public class EnumerableExtensionsTest {
        private Faker _faker;

        [OneTimeSetUp]
        public void Setup() {
            _faker = new Faker();
        }

        [Test]
        public void QueryStringIsProperlyBuiltWithExample() {
            const string expectedStr = "ei=d8XSXrSHNoLC5OUPmvOngAM&q=quuerystringexample&oq=quuerystringexample&" +
                                       "gs_lcp=CgZwc3ktYWIQAzIECAAQDTIECAAQDTIGCAAQDRAeMggIABANEAoQHjIGCAAQDRAeM" +
                                       "gYIABANEB4yCAgAEA0QChAeMggIABANEAoQHjIGCAAQDRAeMgYIABANEB46BwgAEEcQsAM6B" +
                                       "QgAEJECOgQIABBDOgIIADoECAAQCjoGCAAQDRAKUOwvWJVAYO9AaANwAHgCgAGpAogB1x2SA" +
                                       "QYwLjIuMTSYAQCgAQGqAQdnd3Mtd2l6&sclient=psy-ab&ved=0ahUKEwj0noLtudzpAhUC" +
                                       "IbkGHZr5CTAQ4dUDCAw&uact=5";

            var enumerable = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("ei", "d8XSXrSHNoLC5OUPmvOngAM"),
                new KeyValuePair<string, string>("q", "quuerystringexample"),
                new KeyValuePair<string, string>("oq", "quuerystringexample"),
                new KeyValuePair<string, string>("gs_lcp", "CgZwc3ktYWIQAzIECAAQDTIECAAQDTIGCAAQDRAeMggIABANEAoQHj" +
                                                           "IGCAAQDRAeMgYIABANEB4yCAgAEA0QChAeMggIABANEAoQHjIGCAAQ" +
                                                           "DRAeMgYIABANEB46BwgAEEcQsAM6BQgAEJECOgQIABBDOgIIADoECA" +
                                                           "AQCjoGCAAQDRAKUOwvWJVAYO9AaANwAHgCgAGpAogB1x2SAQYwLjIu" +
                                                           "MTSYAQCgAQGqAQdnd3Mtd2l6"),
                new KeyValuePair<string, string>("sclient", "psy-ab"),
                new KeyValuePair<string, string>("ved", "0ahUKEwj0noLtudzpAhUCIbkGHZr5CTAQ4dUDCAw"),
                new KeyValuePair<string, string>("uact", "5"),
            };

            Assert.AreEqual(expectedStr, enumerable.BuildQueryString());
        }

        [Test]
        public void QueryStringIsProperlyBuiltWithRandomAlphaNumericValues() {
            const int paramsCount = 10;
            const int strLen = 10;
            var enumerable = new List<KeyValuePair<string, string>>();
            var expectedStr = "";

            for (var i = 0; i < paramsCount; i++) {
                var separator = i > 0 ? "&" : "";

                // not an integration test, it is just useful in this context too =)
                var key = _faker.Random.AlphaNumeric(strLen);
                var value = _faker.Random.AlphaNumeric(strLen);

                enumerable.Add(new KeyValuePair<string, string>(key, value));
                expectedStr += $"{separator}{key}={value}";
            }

            Assert.AreEqual(expectedStr, enumerable.BuildQueryString());
        }

        [Test]
        public void QueryStringIsProperlyBuiltWithEncodedChars() {
            const string expectedDecodedStr = "q=string ! with @ some = # weird $ chars % for \" testing & " +
                                              "purposes * in ( library )";

            var enumerable = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("q", "string ! with @ some = # weird $ chars % for \" testing & " +
                                                      "purposes * in ( library )"),
            };

            var queryStr = enumerable.BuildQueryString();
            var decodedStr = HttpUtility.UrlDecode(queryStr);
            Assert.AreEqual(expectedDecodedStr, decodedStr);
        }
    }
}