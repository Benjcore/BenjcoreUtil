using Microsoft.VisualStudio.TestTools.UnitTesting;
using util.security;

namespace util.UnitTests.security {

    [TestClass]
    public class simpleMD5Tests {

        [TestMethod]
        public void getMD5Test() {

            string[] testStrings = {
                "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 1234567890",
                "Hello World!", "I quit my job to play a $4 cube game.", "Do you are have pleh gaem?!", "Me when when the.",
                "UCVYVzVmykhr-1w6V662rYNQ", "dQw4w9WgXcQ&t", "j2n#6@G#&WHftE%mF#mVgcZR", "Kneecap"
            };
            string[] expected = {
                "39847c580e6761548747a18b7d82202d",
                "ed076287532e86365e841e92bfc50d8c",
                "a8fd132270fcacdd131e1f5c24f4e679",
                "98ec90b44e74fd388615b8fc996a3f9a",
                "8b9689bb514e1416275c428a17254ec7",
                "c640cc5d0f212ed8555ee15971c2f3bc",
                "cdce2c18a6c4447cb272a94bb5e37390",
                "42a7b8dfb76a6377eddab607d8ceb293",
                "a77d835140bd695414609f13b3079028"
            };

            for (int i = 0; i < testStrings.Length; i++) {
                Assert.AreEqual(expected[i], simpleMD5.getMD5(testStrings[i]).ToLower());
            }

        }

    }

}
