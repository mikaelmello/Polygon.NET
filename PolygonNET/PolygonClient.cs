using PolygonNET.Network;

namespace PolygonNET {
    /// <summary>
    /// Client for the Polygon API.
    /// </summary>
    public class PolygonClient {
        internal readonly IPolygonHttpClient HttpClient;

        public PolygonClient(IPolygonHttpClient polygonHttpClient) {
            HttpClient = polygonHttpClient;
        }
    }
}