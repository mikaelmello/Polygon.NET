using System.Collections.Generic;

namespace PolygonNET.Api {
    /// <summary>
    /// Represents special properties of resource files. Basically, they stand for compile- or run-time resources
    /// for specific file types and asset types. The most important application is IOI-style graders.
    /// Example: {"forTypes":"cpp.*","main":false,"stages":["COMPILE"],"assets":["SOLUTION"]}
    /// </summary>
    public struct PolygonResourceAdvancedProperties {
        /// <summary>
        /// Currently reserved to be false.
        /// </summary>
        public bool Main { get; set; }
        
        /// <summary>
        /// Colon or semicolon separated list of file types this resource if applied, wildcards are supported.
        /// (example: “cpp.*” or “pascal.*;java.11”)
        /// </summary>
        public string ForTypes { get; set; }
        
        /// <summary>
        /// Stages when the resource is applicable.
        /// </summary>
        public List<PolygonResourceStages> Stages { get; set; }
        
        /// <summary>
        /// Asset types where the resource is applicable.
        /// </summary>
        public List<PolygonAssetType> Assets { get; set; }
    }
}
