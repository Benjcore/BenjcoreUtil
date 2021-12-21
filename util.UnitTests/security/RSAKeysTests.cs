using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using util.security;

namespace util.UnitTests.security {

    [TestClass]
    public class RSAKeysTests {

        [TestMethod]
        public void RSAKeysTest() {

            RSAKeys keys = new RSAKeys(false);
            string pub = keys.PublicKeyString;
            string priv = keys.PrivateKeyString;
            Assert.AreEqual(keys.PublicKeyString, RSAKeys.KeyToString(RSAKeys.GetKey(keys.PublicKeyString)));
            Assert.AreEqual(keys.PublicKeyString, RSAKeys.KeyToString(RSAKeys.GetKey(keys.PublicKeyString)));

            string sampleText = "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 1234567890";
            string b64output = RSAKeys.EncryptBase64(sampleText, RSAKeys.GetKey(pub));

            Assert.AreEqual(sampleText, RSAKeys.Decrypt(b64output, RSAKeys.GetKey(priv)));
            Assert.AreEqual(sampleText, RSAKeys.Decrypt(b64output, RSAKeys.GetKey(RSAKeys.KeyToString(RSAKeys.GetKey(priv)))));

            RSACryptoServiceProvider rSAParameters = new RSACryptoServiceProvider();
            rSAParameters.ImportParameters(RSAKeys.GetKey(RSAKeys.KeyToString(RSAKeys.GetKey(pub))));
            b64output = RSAKeys.EncryptBase64(sampleText, rSAParameters.ExportParameters(false));

            Assert.AreEqual(sampleText, RSAKeys.Decrypt(b64output, RSAKeys.GetKey(priv)));
            Assert.AreEqual(sampleText, RSAKeys.Decrypt(b64output, RSAKeys.GetKey(RSAKeys.KeyToString(RSAKeys.GetKey(priv)))));

            Assert.AreEqual(pub, RSAKeys.KeyToString(rSAParameters.ExportParameters(false)));
            // Assert.AreEqual(RSAKeys.GetKey(keys.PublicKeyString), rSAParameters.ExportParameters(false));

            keys.UpdateKeys();
            Assert.AreNotEqual(keys.PublicKeyString, pub);
            Assert.AreNotEqual(keys.PrivateKeyString, priv);

        }

    }

}
