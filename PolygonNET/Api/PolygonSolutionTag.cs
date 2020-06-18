using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Api {
    /// <summary>
    /// Polygon asset types.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PolygonSolutionTag {
        [EnumMember(Value = "MA")] Main,
        [EnumMember(Value = "OK")] Correct,
        [EnumMember(Value = "RJ")] Incorrect,
        [EnumMember(Value = "TL")] TimeLimitExceeded,
        [EnumMember(Value = "TO")] TimeLimitExceededOrCorrect,
        [EnumMember(Value = "TM")] TimeLimitExceededOrMemoryLimitExceeded,
        [EnumMember(Value = "WA")] WrongAnswer,
        [EnumMember(Value = "PE")] PresentationError,
        [EnumMember(Value = "ML")] MemoryLimitExceeded,
        [EnumMember(Value = "RE")] Failed,
    }
}
