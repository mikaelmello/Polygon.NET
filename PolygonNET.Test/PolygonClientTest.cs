using Moq;
using NUnit.Framework;
using PolygonNET.Network;

namespace PolygonNET.Test {
    [TestFixture]
    public class PolygonClientTest {
        [Test]
        [Description("PolygonClient constructor properly assigns internal http client")]
        public void ConstructorWorks() {
            var mockClient = new Mock<IPolygonHttpClient>();
            var polygon = new PolygonClient(mockClient.Object);

            Assert.AreSame(mockClient.Object, polygon.HttpClient);
        }
    }
}