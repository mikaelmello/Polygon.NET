namespace PolygonNET.Api {
    /// <summary>
    /// Represents a problem’s general information.
    /// </summary>
    public struct PolygonProblemInfo {
        /// <summary>
        /// Input file name or "stdin" for standard input.
        /// </summary>    
        public string InputFile { get; set; }
        
        /// <summary>
        /// Output file name or "stdout" for standard output.
        /// </summary>
        public string OutputFile { get; set; }
        
        /// <summary>
        /// Whether the problem is interactive.
        /// </summary>
        public bool Interactive { get; set; }
        
        /// <summary>
        /// Time limit per test in milliseconds (between 250 ms and 15000 ms).
        /// </summary>
        public long TimeLimit { get; set; }
        
        /// <summary>
        /// Memory limit in MB (between 4 MB and 1024 MB).
        /// </summary>
        public long MemoryLimit { get; set; }
    }
}