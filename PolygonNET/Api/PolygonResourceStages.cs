using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Api {
    /// <summary>
    /// Phase when the resource is applicable.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PolygonResourceStages {
        Compile,
        Run,
    }
}