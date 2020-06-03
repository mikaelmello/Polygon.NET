using System;
using System.Linq;
using Bogus;
using NUnit.Framework;
using PolygonNET.Utils;

namespace PolygonNET.Test.Utils {
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
        public void RandomAlphanumericStringThrowsWithInvalidArgs() {
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomAlphanumericString(-1));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomAlphanumericString(-100));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomAlphanumericString(1_000_000_000));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomAlphanumericString(268_435_456));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomAlphanumericString(-1));
        }

        [Test]
        public void RandomAlphanumericStringDoesNotThrowWithEdgeCaseArgs() {
            Assert.DoesNotThrow(() => _randomUtils.GetRandomAlphanumericString(0));
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
            const string charset = "abcde12345!@#$%[]{}()";

            for (var i = 0; i < _randIterations; i++) {
                var length = _faker.Random.Number(_maxStringLength);
                var rand = _randomUtils.GetRandomString(length, charset.ToHashSet());

                var inCharset = rand.All(c => charset.Contains(c));
                Assert.IsTrue(inCharset);
            }
        }

        [Test]
        public void RandomStringThrowsWithInvalidArgs() {
            var charset = "abcde12345!@#$%[]{}()".ToHashSet();

            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomString(-1, charset));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomString(-100, charset));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomString(1_000_000_000, charset));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomString(268_435_456, charset));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomString(10, "".ToHashSet()));
            Assert.Throws<ArgumentException>(() => _randomUtils.GetRandomString(-1, "".ToHashSet()));

            Assert.Throws<ArgumentNullException>(() => _randomUtils.GetRandomString(10, null));
        }

        [Test]
        public void RandomStringDoesNotThrowWithEdgeCaseArgs() {
            var charset = "abcde12345!@#$%[]{}()".ToHashSet();

            Assert.DoesNotThrow(() => _randomUtils.GetRandomString(0, charset));
            Assert.DoesNotThrow(() => _randomUtils.GetRandomString(10, "a".ToHashSet()));
        }
    }
}