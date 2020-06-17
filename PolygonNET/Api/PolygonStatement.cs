namespace PolygonNET.Api {
    /// <summary>
    /// Represents a problem’s statement.
    /// </summary>
    public struct PolygonStatement {
        /// <summary>
        /// Name of the problem in the statement's language.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Legend of the problem in the statement's language.
        /// </summary>
        public string Legend { get; set; }

        /// <summary>
        /// Input format of the problem in the statement's language.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Output format of the problem in the statement's language.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Statement encoding in packages.
        /// </summary>
        public string Encoding { get; set; }
    }
}