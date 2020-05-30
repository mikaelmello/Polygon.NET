using NUnit.Framework;
using PolygonNET.Utils;

namespace PolygonNET.Tests.Utils {
    [TestFixture]
    public class CryptoUtilsTest {
        private ICryptoUtils _cryptoUtils;

        [OneTimeSetUp]
        public void Setup() {
            _cryptoUtils = new CryptoUtils();
        }
        
        [Test]
        public void LoremIpsumHashMatches() {
            // ReSharper disable StringLiteralTypo
            const string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor" +
                                " incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
                                "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute " +
                                "irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla " +
                                "pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia " +
                                "deserunt mollit anim id est laborum.";
            const string expectedHash = "8BA760CAC29CB2B2CE66858EAD169174057AA1298CCD581514E6DB6DEE3285280EE6E3A54C" +
                                        "9319071DC8165FF061D77783100D449C937FF1FB4CD1BB516A69B9";
            var calculatedHash = _cryptoUtils.ComputeSha512Hash(text);

            Assert.AreEqual(expectedHash, calculatedHash);
        }

        [Test]
        public void ExpectedStringTemplateHashMatches() {
            // ReSharper disable StringLiteralTypo
            const string text = "<rand>/<methodName>?param1=value1&param2=value2...&paramN=valueN#<secret>";
            const string expectedHash = "2A47D4BE55EFE9CF64C8FDB16C748640C49398A3768B082DA73CE61CD9CDB37A27A9D184" +
                                        "3678B2EBE94A0083AAE5686DADC713D812AF0C3CA469CF2BFEB6DF5C";
            var calculatedHash = _cryptoUtils.ComputeSha512Hash(text);

            Assert.AreEqual(expectedHash, calculatedHash);
        }
    }
}