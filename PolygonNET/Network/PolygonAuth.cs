using System;
using System.Collections.Generic;
using System.Linq;
using PolygonNET.Utils;

namespace PolygonNET.Network {
    public static class PolygonAuth {
        public static Dictionary<string, string> AuthorizeRequest(string methodName,
                                                                  Dictionary<string, string> parameters,
                                                                  string apiKey, string apiSecret) {
            parameters["apiKey"] = apiKey;
            parameters["time"] = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            parameters["apiSig"] = GetApiSignature(methodName, parameters, apiSecret);

            return parameters;
        }

        private static string GetApiSignature(string methodName, Dictionary<string, string> parameters,
                                              string apiSecret) {
            var rand = RandomUtils.GetRandomAlphanumericString(6);

            var orderedParams = parameters.OrderBy(pair => pair.Key);
            var queryString = orderedParams.BuildQueryString();
            var plainText = $"{rand}/{methodName}?{queryString}#{apiSecret}";
            var hash = CryptoUtils.ComputeSha512Hash(plainText);
            return hash;
        }
    }
}