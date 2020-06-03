using PolygonNET.Network;

namespace PolygonNET {
    public class PolygonClient {
        internal readonly IPolygonHttpClient HttpClient;

        public PolygonClient(IPolygonHttpClient polygonHttpClient) {
            HttpClient = polygonHttpClient;
        }
    }
}