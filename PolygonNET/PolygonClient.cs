using PolygonNET.Api;
using PolygonNET.Methods;
using PolygonNET.Network;

namespace PolygonNET {
    /// <summary>
    /// Client for the Polygon API.
    /// </summary>
    public class PolygonClient {
        internal readonly IPolygonHttpClient HttpClient;

        public PolygonClient(IPolygonHttpClient polygonHttpClient) {
            HttpClient = polygonHttpClient;
            Problems = new Problems(HttpClient);
        }

        /// <summary>
        /// Interface to methods in the <see cref="Methods.Problems" /> namespace.
        /// </summary>
        public Problems Problems { get; }

        /// <summary>
        /// Creates a <see cref="Methods.Contest" /> object from and id and an optional pin code,
        /// allowing access to contest-specific methods.
        /// </summary>
        /// <param name="contestId">Contest id.</param>
        /// <param name="pinCode">Pin code for the contest.</param>
        public Contest Contest(long contestId, string pinCode = null) {
            return new Contest(HttpClient, contestId, pinCode);
        }

        /// <summary>
        /// Creates a <see cref="Methods.Problem" /> object from and id and an optional pin code,
        /// allowing access to problem-specific methods.
        /// </summary>
        /// <param name="problemId">Problem id.</param>
        /// <param name="pinCode">Pin code for the problem.</param>
        public Problem Problem(long problemId, string pinCode = null) {
            return new Problem(HttpClient, problemId, pinCode);
        }

        /// <summary>
        /// Creates a <see cref="Methods.Problem" /> object from a <see cref="PolygonProblem" />
        /// and an optional pin code, allowing access to problem-specific methods.
        /// </summary>
        /// <param name="problem">
        /// <see cref="PolygonProblem" /> containing information about the problem that you wish to access.
        /// </param>
        /// <param name="pinCode">Pin code for the problem.</param>
        public Problem Problem(PolygonProblem problem, string pinCode = null) {
            return Problem(problem.Id, pinCode);
        }
    }
}