using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Network {
    /// <summary>
    /// Possible response status of API requests.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PolygonResponseStatus {
        Ok,
        Failed,
    }

    /// <summary>
    /// General structure of the response from a request to the Polygon API.
    /// </summary>
    /// <typeparam name="T">Type of the result.</typeparam>
    public sealed class PolygonResponse<T> {
        /// <summary>
        /// Status of the response. <see cref="PolygonResponseStatus.Ok" /> if the request has succeeded,
        /// <see cref="PolygonResponseStatus.Failed" /> if not.
        /// </summary>
        public PolygonResponseStatus Status { get; set; }

        /// <summary>
        /// If the request has failed, it contains the reason, null otherwise.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Result of the request.
        /// </summary>
        public T Result { get; set; }
    }

    /// <summary>
    /// Structure of the response of a failed request to the Polygon API.
    /// </summary>
    public sealed class PolygonFailedResponse {
        /// <summary>
        /// Status of the response. <see cref="PolygonResponseStatus.Ok" /> if the request has succeeded,
        /// <see cref="PolygonResponseStatus.Failed" /> if not.
        /// </summary>
        public PolygonResponseStatus Status { get; set; }

        /// <summary>
        /// If the request has failed, it contains the reason, null otherwise.
        /// </summary>
        public string Comment { get; set; }
    }
}