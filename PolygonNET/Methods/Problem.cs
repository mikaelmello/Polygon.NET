using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PolygonNET.Api;
using PolygonNET.Network;
using PolygonNET.Network.Exceptions;

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
        /// Retrieves the <see cref="PolygonProblemInfo" /> of the problem.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// </exception>
        public async Task<PolygonProblemInfo> GetInfo(CancellationToken ct = default) {
            const string methodName = "problem.info";

            var parameters = DefaultParameters();

            return await _client.RequestAsync<PolygonProblemInfo>(methodName, parameters, ct);
        }

        /// <summary>
        /// Updates information about the problem.
        /// </summary>
        /// <param name="inputFile">Input file name or "stdin" for standard input. Not updated if null or whitespace.</param>
        /// <param name="outputFile">Output file name or "stdout" for standard output. Not updated if null or whitespace.</param>
        /// <param name="interactive">Whether the problem is interactive. Not updated if null.</param>
        /// <param name="memoryLimit">Time limit per test in milliseconds (between 250 ms and 15000 ms). Not updated if null.</param>
        /// <param name="timeLimit">Memory limit in MB (between 4 MB and 1024 MB). Not updated if null.</param>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>The updated <see cref="PolygonProblemInfo" /> of the problem.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// </exception>
        public async Task UpdateInfo(string inputFile = null,
                                     string outputFile = null,
                                     bool? interactive = null,
                                     long? memoryLimit = null,
                                     long? timeLimit = null,
                                     CancellationToken ct = default) {
            const string methodName = "problem.updateInfo";

            var parameters = DefaultParameters();

            if (!string.IsNullOrWhiteSpace(inputFile)) parameters.Add("inputFile", inputFile);
            if (!string.IsNullOrWhiteSpace(outputFile)) parameters.Add("outputFile", outputFile);
            if (interactive.HasValue) parameters.Add("interactive", interactive.ToString());
            if (memoryLimit.HasValue) parameters.Add("memoryLimit", memoryLimit.ToString());
            if (timeLimit.HasValue) parameters.Add("timeLimit", timeLimit.ToString());

            await _client.RequestAsync(methodName, parameters, ct);
        }

        /// <summary>
        /// Updates information about the problem.
        /// </summary>
        /// <param name="problemInfo">
        /// Object containing properties that are to be updated. Null or whitespace values are not
        /// updated.
        /// </param>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>The updated <see cref="PolygonProblemInfo" /> of the problem.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// </exception>
        public Task UpdateInfo(PolygonProblemInfo problemInfo,
                               CancellationToken ct = default) {
            return UpdateInfo(problemInfo.InputFile, problemInfo.OutputFile, problemInfo.Interactive,
                              problemInfo.MemoryLimit, problemInfo.TimeLimit, ct);
        }

        /// <summary>
        /// Returns the name of currently set checker of the problem.
        /// </summary>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>The name of currently set checker of the problem.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// </exception>
        public async Task<string> GetCheckerName(CancellationToken ct = default) {
            const string methodName = "problem.checker";

            var parameters = DefaultParameters();

            return await _client.RequestAsync<string>(methodName, parameters, ct);
        }

        /// <summary>
        /// Returns the name of currently set validator of the problem. If the response is an empty string,
        /// it is interpreted as no validator and null is returned.
        /// </summary>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>The name of currently set validator of the problem.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// </exception>
        public async Task<string> GetValidatorName(CancellationToken ct = default) {
            const string methodName = "problem.validator";

            var parameters = DefaultParameters();

            var validatorName = await _client.RequestAsync<string>(methodName, parameters, ct);

            return string.IsNullOrEmpty(validatorName) ? null : validatorName;
        }

        /// <summary>
        /// Returns the name of currently set interactor of the problem. If the problem is not interactive, the request
        /// will fail.
        /// </summary>
        /// <param name="ct">Cancellation token for the request.</param>
        /// <returns>The name of currently set interactor of the problem.</returns>
        /// <exception cref="PolygonFailedRequestException">
        /// When the request fails, containing as the message the reason for the request to fail.
        /// </exception>
        public async Task<string> GetInteractorName(CancellationToken ct = default) {
            const string methodName = "problem.interactor";

            var parameters = DefaultParameters();

            return await _client.RequestAsync<string>(methodName, parameters, ct);
        }
    }
}
