using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolygonNET.Utils {
    public static class EnumerableExtensions {
        public static string BuildQueryString(this IEnumerable<KeyValuePair<string, string>> parameters) {
            var strPairs = parameters.Select(kvp =>
                                                 $"{HttpUtility.UrlEncode(kvp.Key)}={HttpUtility.UrlEncode(kvp.Value)}");

            var queryStr = string.Join("&", strPairs);
            return queryStr;
        }
    }
}