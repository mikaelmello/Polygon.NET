using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Api {
    /// <summary>
    /// User's access type to a problem.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PolygonAccessType {
        Read,
        Write,
        Owner,
    }
}
