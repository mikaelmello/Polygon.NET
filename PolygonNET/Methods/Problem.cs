using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PolygonNET.Api;
using PolygonNET.Network;

namespace PolygonNET.Methods {
    /// <summary>
    /// Interface of Problem methods.
    /// </summary>
    public class Problem {
        private readonly IPolygonHttpClient _client;
        private readonly long _problemId;
        private readonly string _pinCode;

        internal Problem(IPolygonHttpClient client, long problemId, string pinCode) {
            _client = client;
            _pinCode = pinCode;
            _problemId = problemId;
        }

        /// <summary>
        /// Default parameters present in all requests made by methods in this class.
        /// </summary>
        private Dictionary<string, string> DefaultParameters() {
            var parameters = new Dictionary<string, string> {
                {"problemId", _problemId.ToString()},
            };

            if (!string.IsNullOrWhiteSpace(_pinCode)) parameters.Add("pin", _pinCode);

            return parameters;
        }

        /// <summary>
        /// Retrieves the <see cref="PolygonProblemInfo"/> of the problem.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        public async Task<PolygonProblemInfo> GetProblemInfo(CancellationToken ct = default) {
            const string methodName = "problem.info";

            var parameters = DefaultParameters();

            return await _client.RequestAsync<PolygonProblemInfo>(methodName, parameters, ct);
        }
    }
}