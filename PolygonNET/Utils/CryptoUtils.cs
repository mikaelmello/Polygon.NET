using System;
using System.Security.Cryptography;
using System.Text;

namespace PolygonNET.Utils {
    public static class CryptoUtils {
        public static string ComputeSha512Hash(string plainText) {
            using var sha512 = new SHA512Managed();
            var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}