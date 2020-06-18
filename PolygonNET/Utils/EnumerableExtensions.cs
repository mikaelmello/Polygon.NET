using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolygonNET.Utils {
    internal static class EnumerableExtensions {
        /// <summary>
        /// Builds a query string by concatenating the key-value pairs using the format &quot;key=value&quot;.
        /// Each pair is separated by an &amp; character and both key and value are url-encoded.
        /// </summary>
        /// <param name="parameters">Key-value pairs to be serialized into a query string.</param>
        /// <returns>String containing url-encoded key-value pairs separated by &amp;.</returns>
        public static string BuildQueryString(this IEnumerable<KeyValuePair<string, string>> parameters) {
            var strPairs = parameters.Select(kvp =>
                                                 $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}");

            var queryStr = string.Join("&", strPairs);
            return queryStr;
        }
    }
}
