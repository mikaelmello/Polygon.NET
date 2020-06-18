using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PolygonNET.Api {
    /// <summary>
    /// Represents a resource, source or aux file.
    /// </summary>
    public struct PolygonProblemSolution {
        /// <summary>
        /// Name of the solution.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Length of the solution.
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// Last time when the solution     was modified.
        /// </summary>
        [JsonProperty("modificationTimeSeconds")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset ModifiedAt { get; set; }

        /// <summary>
        /// Source file type.
        /// </summary>
        public string SourceType { get; set; }

        /// <summary>
        /// Solution tag.
        /// </summary>
        public PolygonSolutionTag Tag { get; set; }
    }
}
