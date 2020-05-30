using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace PolygonNET.Utils {
    public interface IRandomUtils {
        /// <summary>
        /// Generates a string with <paramref name="length"/> random alphanumeric characters.
        /// </summary>
        /// <param name="length">Length of the generated string.</param>
        /// <returns>String with <paramref name="length"/> random alphanumeric  characters.</returns>
        public string GetRandomAlphanumericString(int length);
        
        /// <summary>
        /// Generates a string with <paramref name="length"/> random characters from <paramref name="characterSet"/>.
        /// </summary>
        /// <param name="length">Length of the generated string.</param>
        /// <param name="characterSet">Set of characters that the string can contain.</param>
        /// <returns>
        /// String with <paramref name="length"/> random characters from <paramref name="characterSet"/>.
        /// </returns>
        public string GetRandomString(int length, ISet<char> characterSet);
    }
    
    public class RandomUtils : IRandomUtils {
        public string GetRandomAlphanumericString(int length) {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";
            return GetRandomString(length, alphanumericCharacters.ToHashSet());
        }

        public string GetRandomString(int length, ISet<char> characterSet) {
            if (length < 0)
                throw new ArgumentException("length must not be negative", nameof(length));
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", nameof(length));
            if (characterSet == null)
                throw new ArgumentNullException(nameof(characterSet));

            var characterArray = characterSet.ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", nameof(characterSet));

            var bytes = new byte[length * 8];
            var result = new char[length];
            using (var cryptoProvider = new RNGCryptoServiceProvider()) {
                cryptoProvider.GetBytes(bytes);
            }

            for (var i = 0; i < length; i++) {
                var value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint) characterArray.Length];
            }

            return new string(result);
        }
    }
}