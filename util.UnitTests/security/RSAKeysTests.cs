using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            keys.UpdateKeys();
            Assert.AreNotEqual(keys.PublicKeyString, pub);
            Assert.AreNotEqual(keys.PrivateKeyString, priv);

        }

    }

}
