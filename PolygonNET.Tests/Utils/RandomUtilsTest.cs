using System.Linq;
using Bogus;
using NUnit.Framework;
using PolygonNET.Utils;

namespace PolygonNET.Tests.Utils {
    [TestFixture]
    public class RandomUtilsTest {
        private Faker _faker;
        private RandomUtils _randomUtils;
        private int _randIterations;
        private int _maxStringLength;

        [OneTimeSetUp]
        public void Setup() {
            _faker = new Faker();
            _randomUtils = new RandomUtils();
            _randIterations = 10;
            _maxStringLength = 500;
        }

        [Test]
        public void RandomAlphaNumericStringReturnsCorrectLength() {
            for (var i = 0; i < _randIterations; i++) {
                var length = _faker.Random.Number(_maxStringLength);
                var rand = _randomUtils.GetRandomAlphanumericString(length);

                Assert.AreEqual(length, rand.Length);
            }
        }

        [Test]
        public void RandomAlphaNumericStringContainsAlphaNumericCharsOnly() {
            for (var i = 0; i < _randIterations; i++) {
                var length = _faker.Random.Number(_maxStringLength);
                var rand = _randomUtils.GetRandomAlphanumericString(length);

                var alphaOnly = rand.All(char.IsLetterOrDigit);
                Assert.IsTrue(alphaOnly);
            }
        }

        [Test]
        public void RandomStringReturnsCorrectLength() {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";

            for (var i = 0; i < _randIterations; i++) {
                var length = _faker.Random.Number(_maxStringLength);
                var rand = _randomUtils.GetRandomString(length, alphanumericCharacters.ToHashSet());

                Assert.AreEqual(length, rand.Length);
            }
        }

        [Test]
        public void RandomStringReturnsCorrectCharset() {
            // ReSharper disable once StringLiteralTypo
            const string charset = "abcde12345!@#$%[]{}()";

            for (var i = 0; i < _randIterations; i++) {
                var length = _faker.Random.Number(_maxStringLength);
                var rand = _randomUtils.GetRandomString(length, charset.ToHashSet());

                var inCharset = rand.All(c => charset.Contains(c));
                Assert.IsTrue(inCharset);
            }
        }
    }
}