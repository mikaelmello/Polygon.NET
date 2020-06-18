namespace PolygonNET.Api {
    /// <summary>
    /// Represents a Polygon problem.
    /// </summary>
    public struct PolygonProblem {
        /// <summary>
        /// Problem Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Handle of the problem's owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// User's access type for the problem.
        /// </summary>
        public PolygonAccessType PolygonAccessType { get; set; }

        /// <summary>
        /// Current problem revision.
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Latest revision with package available (may be absent).
        /// </summary>
        public int? LatestPackage { get; set; }

        /// <summary>
        /// Name of the problem.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether the problem has been deleted.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Whether the problem has been modified.
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Whether the problem is in the user's favorites.
        /// </summary>
        public bool Favorite { get; set; }
    }
}
