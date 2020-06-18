using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Api {
    /// <summary>
    /// Polygon asset types.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PolygonAssetType {
        Validator,
        Interactor,
        Checker,
        Solution,
    }
}
