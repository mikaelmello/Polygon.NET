using System;
using System.Collections.Generic;
using System.Linq;
using PolygonNET.Utils;

namespace PolygonNET.Network {
    public interface IPolygonAuth {
        /// <summary>
        /// Adds the apiKey, time and apiSig parameters in the <paramref name="parameters"/> dictionary, necessary
        /// for auth in the Polygon API.
        /// </summary>
        /// <param name="methodName">Name of the method called by the request.</param>
        /// <param name="parameters">Parameters containing in the request.</param>
        /// <param name="apiKey">Polygon API Key</param>
        /// <param name="apiSecret">Polygon API Secret</param>
        public void AuthorizeRequest(string methodName, Dictionary<string, string> parameters, string apiKey,
                                     string apiSecret);

        /// <summary>
        /// Returns the signature needed for auth in the Polygon API, based on the <paramref name="methodName"/>,
        /// <paramref name="parameters"/> and <paramref name="apiSecret"/>.
        /// </summary>
        /// <param name="methodName">Name of the method called by the request.</param>
        /// <param name="parameters">Parameters containing in the request.</param>
        /// <param name="apiSecret">Polygon API Secret</param>
        public string GetApiSignature(string methodName, Dictionary<string, string> parameters, string apiSecret);
    }

    public class PolygonAuth : IPolygonAuth {
        private readonly IPolygonRandomUtils _randomUtils;
        private readonly IPolygonCryptoUtils _cryptoUtils;

        public PolygonAuth(IPolygonRandomUtils randomUtils, IPolygonCryptoUtils cryptoUtils) {
            _randomUtils = randomUtils;
            _cryptoUtils = cryptoUtils;
        }

        public PolygonAuth() {
            _randomUtils = new PolygonRandomUtils();
            _cryptoUtils = new PolygonCryptoUtils();
        }

        public void AuthorizeRequest(string methodName,
                                     Dictionary<string, string> parameters,
                                     string apiKey, string apiSecret) {
            parameters["apiKey"] = apiKey;
            parameters["time"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            parameters["apiSig"] = GetApiSignature(methodName, parameters, apiSecret);
        }

        public string GetApiSignature(string methodName, Dictionary<string, string> parameters,
                                      string apiSecret) {
            var rand = _randomUtils.GetRandomAlphanumericString(6);

            var orderedParams = parameters.OrderBy(pair => pair.Key);
            var queryString = orderedParams.BuildQueryString();
            var plainText = $"{rand}/{methodName}?{queryString}#{apiSecret}";
            var hash = _cryptoUtils.ComputeSha512Hash(plainText).ToLower();
            return $"{rand}{hash}";
        }
    }
}