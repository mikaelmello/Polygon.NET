using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Api {
    /// <summary>
    /// Represents a resource, source or aux file.
    /// </summary>
    public struct PolygonFile {
        /// <summary>
        /// Name of the file.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Length of the file.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Last time when the file was modified.
        /// </summary>
        [JsonProperty("modificationTimeSeconds")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset ModifiedAt { get; set; }

        /// <summary>
        /// Source file type. Present only for source files.
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// Extra properties resource files can have. (may be absent)
        /// </summary>
        [JsonProperty("resourceAdvancedProperties")]
        public PolygonResourceAdvancedProperties Properties { get; set; }
    }
}
