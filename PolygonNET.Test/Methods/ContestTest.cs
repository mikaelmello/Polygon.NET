using System.Collections.Generic;
using System.Threading;
using Bogus;
using Moq;
using NUnit.Framework;
using PolygonNET.Api;
using PolygonNET.Methods;
using PolygonNET.Network;

namespace PolygonNET.Test.Methods {
    [TestFixture]
    public class ContestTest {
        private Faker _faker;
        private Mock<IPolygonHttpClient> _polygonHttpClient;

        private long _contestId;
        private string _pinCode;
        private Contest _contest;

        [OneTimeSetUp]
        public void OneTimeSetUp() {
            _faker = new Faker();
        }

        [SetUp]
        public void SetUp() {
            _contestId = _faker.Random.Long();
            _pinCode = _faker.Random.Hexadecimal(6);
            _polygonHttpClient = new Mock<IPolygonHttpClient>();
            _contest = new Contest(_polygonHttpClient.Object, _contestId, _pinCode);
        }

        [Test]
        [Description("Constructor works")]
        public void ConstructorWorks() {
            Assert.DoesNotThrow(() => new Contest(_polygonHttpClient.Object, _contestId, null));
            Assert.DoesNotThrow(() => new Contest(_polygonHttpClient.Object, _contestId, _pinCode));
        }

        [Test]
        [Description("GetProblems calls request with correct method name")]
        public void GetProblemsHasCorrectMethodName() {
            const string expectedMethodName = "contest.problems";

            _contest.GetProblems();

            _polygonHttpClient.Verify(
                c => c.RequestAsync<List<PolygonProblem>>(expectedMethodName,
                                                          It.IsAny<Dictionary<string, string>>(),
                                                          It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        [Description("GetProblems pass cancellation token to request")]
        public void GetProblemsHasCorrectCt() {
            var cancellationToken = new CancellationTokenSource().Token;

            _contest.GetProblems(cancellationToken);

            _polygonHttpClient.Verify(
                c => c.RequestAsync<List<PolygonProblem>>(It.IsAny<string>(),
                                                          It.IsAny<Dictionary<string, string>>(),
                                                          cancellationToken),
                Times.Once);
        }

        [Test]
        [Description("GetProblems pass correct params to request")]
        public void GetProblemsHasCorrectParams() {
            var expectedParams = new Dictionary<string, string> {
                {"pin", _pinCode},
                {"contestId", _contestId.ToString()},
            };

            Dictionary<string, string> parameters = null;

            _polygonHttpClient.Setup(c => c.RequestAsync<List<PolygonProblem>>(
                                         It.IsAny<string>(),
                                         It.IsAny<Dictionary<string, string>>(),
                                         It.IsAny<CancellationToken>()))
                              .Callback<string, Dictionary<string, string>, CancellationToken>(
                                  (m, p, c) => parameters = p);

            _contest.GetProblems();

            _polygonHttpClient.Verify(
                c => c.RequestAsync<It.IsAnyType>(It.IsAny<string>(),
                                                  It.IsAny<Dictionary<string, string>>(),
                                                  It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.NotNull(parameters);
            Assert.AreEqual(expectedParams, parameters);
        }

        [Test]
        [Description("GetProblems pass correct params to request when pin code is null")]
        public void GetProblemsHasCorrectParamsWithNullPin() {
            _contest = new Contest(_polygonHttpClient.Object, _contestId, null);

            var expectedParams = new Dictionary<string, string> {
                {"contestId", _contestId.ToString()},
            };

            Dictionary<string, string> parameters = null;

            _polygonHttpClient.Setup(c => c.RequestAsync<List<PolygonProblem>>(
                                         It.IsAny<string>(),
                                         It.IsAny<Dictionary<string, string>>(),
                                         It.IsAny<CancellationToken>()))
                              .Callback<string, Dictionary<string, string>, CancellationToken>(
                                  (m, p, c) => parameters = p);

            _contest.GetProblems();

            _polygonHttpClient.Verify(
                c => c.RequestAsync<It.IsAnyType>(It.IsAny<string>(),
                                                  It.IsAny<Dictionary<string, string>>(),
                                                  It.IsAny<CancellationToken>()),
                Times.Once);

            Assert.NotNull(parameters);
            Assert.AreEqual(expectedParams, parameters);
        }
    }
}
