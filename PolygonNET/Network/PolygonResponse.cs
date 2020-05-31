using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Network {
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PolygonResponseStatus {
        Ok,
        Failed,
    }

    public sealed class PolygonResponse<T> {
        public PolygonResponseStatus Status { get; set; }
        public string Comment { get; set; }
        public T Result { get; set; }
    }

    public sealed class PolygonFailedResponse {
        public PolygonResponseStatus Status { get; set; }
        public string Comment { get; set; }
    }
}