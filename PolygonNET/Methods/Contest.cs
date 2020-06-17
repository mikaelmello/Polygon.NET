using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PolygonNET.Api;
using PolygonNET.Network;

namespace PolygonNET.Methods {
    /// <summary>
    /// Interface of Contest methods.
    /// </summary>
    public class Contest {
        private readonly IPolygonHttpClient _client;
        private readonly long _contestId;
        private readonly string _pinCode;

        internal Contest(IPolygonHttpClient client, long contestId, string pinCode) {
            _client = client;
            _pinCode = pinCode;
            _contestId = contestId;
        }

        /// <summary>
        /// Default parameters present in all requests made by methods in this class.
        /// </summary>
        private Dictionary<string, string> DefaultParameters() {
            var parameters = new Dictionary<string, string> {
                {"contestId", _contestId.ToString()},
            };

            if (!string.IsNullOrWhiteSpace(_pinCode)) parameters.Add("pin", _pinCode);

            return parameters;
        }

        /// <summary>
        /// Returns a list of <see cref="PolygonProblem"/> with information about the problems of the contest.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        public async Task<List<PolygonProblem>> GetProblems(CancellationToken ct = default) {
            const string methodName = "contest.problems";

            var parameters = DefaultParameters();

            return await _client.RequestAsync<List<PolygonProblem>>(methodName, parameters, ct);
        }
    }
}