namespace PolygonNET {
    /// <summary>
    /// Configuration used when communicating with the Polygon API.
    /// </summary>
    public class PolygonConfiguration {
        /// <summary>
        /// User's API key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// User's API secret.
        /// </summary>
        public string ApiSecret { get; set; }

        /// <summary>
        /// Base URL of the Polygon API, defaults to "https://polygon.codeforces.com/api/".
        /// </summary>
        public string ApiBaseUrl { get; set; } = "https://polygon.codeforces.com/api/";
    }
}