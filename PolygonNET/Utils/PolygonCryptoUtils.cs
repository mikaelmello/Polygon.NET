using System;
using System.Security.Cryptography;
using System.Text;

namespace PolygonNET.Utils {
    /// <summary>
    /// Utilities methods related to crypto used throughout the library.
    /// </summary>
    public interface IPolygonCryptoUtils {
        /// <summary>
        /// Computes the SHA512 hash of <paramref name="plainText"/> and returns its equivalent hexadecimal
        /// representation, uppercase and stripped of dashes.
        /// </summary>
        /// <param name="plainText">Text to be hashed</param>
        /// <returns>Equivalent hexadecimal representation of the SHA512 hash of <paramref name="plainText"/>,
        /// uppercase and stripped of dashes.</returns>
        public string ComputeSha512Hash(string plainText);
    }

    internal class PolygonCryptoUtils : IPolygonCryptoUtils {
        public string ComputeSha512Hash(string plainText) {
            using var sha512 = new SHA512Managed();
            var hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}