using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PolygonNET.Api;
using PolygonNET.Network;

namespace PolygonNET.Methods {
    public class Problems {
        private readonly IPolygonHttpClient _client;

        internal Problems(IPolygonHttpClient client) {
            _client = client;
        }

        /// <summary>
        /// Returns a list of problems, available to the user, according to search parameters.
        /// </summary>
        /// <param name="showDeleted">Whether to show deleted problems (defaults to false).</param>
        /// <param name="problemId">Problem id.</param>
        /// <param name="problemName">Problem name. Ignored if it is null or only whitespaces.</param>
        /// <param name="problemOwner">Problem owner login. Ignored if it is null or only whitespaces.</param>
        /// <param name="ct">Cancellation token used in the request.</param>
        /// <returns>A list of problems, available to the user, according to search parameters.</returns>
        public async Task<List<PolygonProblem>> GetAllAvailable(bool? showDeleted = null,
                                                                long? problemId = null,
                                                                string problemName = null,
                                                                string problemOwner = null,
                                                                CancellationToken ct = default) {
            const string methodName = "problems.list";

            var parameters = new Dictionary<string, string>();

            if (problemId.HasValue) parameters.Add("id", problemId.ToString());
            if (showDeleted.HasValue) parameters.Add("showDeleted", showDeleted.ToString());
            if (!string.IsNullOrWhiteSpace(problemName)) parameters.Add("name", problemName);
            if (!string.IsNullOrWhiteSpace(problemOwner)) parameters.Add("owner", problemOwner);

            return await _client.RequestAsync<List<PolygonProblem>>(methodName, parameters, ct);
        }
    }
}
