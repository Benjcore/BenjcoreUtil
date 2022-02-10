using System.Text;
using BenjcoreUtil.Security.Hashing;
// ReSharper disable StringLiteralTypo

namespace UnitTests.Security.Hashing; 

[TestClass]
public class HashingTests {
  
  public static string[] TestStrings = {
    "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 1234567890",
    "Hello World!", "I quit my job to play a $4 cube game.", "Do you are have pleh gaem?!", "Me when when the.",
    "UCVYVzVmykhr-1w6V662rYNQ", "dQw4w9WgXcQ&t", "j2n#6@G#&WHftE%mF#mVgcZR", "Kneecap"
  };
  
  public static string[] ExpectedMD5s = {
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
  
  public static string[] ExpectedSHA1s = {
    "80e970c71526f57c8fb4585b1f015dc5e67ccc45",
    "2ef7bde608ce5404e97d5f042f95f89f1c232871",
    "edcc00b5587780f3cb75bfb6fed15a5111d49eda",
    "2f3cacb4c4a77dcf365b4de299fe9e65501e1b29",
    "6f65a52811750196308bc8abd8aa8ee4c2b7fea6",
    "1de0d67cd5733feade92baae8f8812df3258445d",
    "f2149c728cdc663f762e27f98dcd9d84a6e90023",
    "577c3bbccb40dd377639263aa6626671a5ba955b",
    "64a1d8478b6f9ae8608ff39fa9748329d4ee45b5"
  };
  
  public static string[] ExpectedSHA256s = {
    "d73b6ed72ec8e3b77c8cab7358af99031e352564f5b60fb6beef9027f293448e",
    "7f83b1657ff1fc53b92dc18148a1d65dfc2d4b1fa3d677284addd200126d9069",
    "e74fbfc469f413f52081555c212800de11cd196aff2226eeb1e43c252b46c0a7",
    "9db4c96a4722aa5c409e6fd1a5710c5dcf95080575d7c1e3d170519437127cb9",
    "c8cd46c6fd8e4527efbf72c3a0261343f0463204b5dbce810b11a752cc0c9974",
    "8aafc42cb012403817949fd5f20df4e2f1a1c643e23086a186e6d47ac77aa3f1",
    "a0289b5baafdaebd00b8d24289a49a5c8a3848b4798825c26b50dec75989b409",
    "a0785418cac16272373197c616946a336e6d3ab17c59f13cd0537579b6b82850",
    "21528c70549334782ea5c83719f4d7f5d38b08342555f67fea7c50c302815e1b"
  };
  
  public static string[] ExpectedSHA512s = {
    "eb496528f6d04039426921dd3df5668a2d0eab8187b4d051938b29dd42f9704ea2a3b0a336d29c8cff77a986db02ff9cc8c6ca869ec3729e50af316783447498",
    "861844d6704e8573fec34d967e20bcfef3d424cf48be04e6dc08f2bd58c729743371015ead891cc3cf1c9d34b49264b510751b1ff9e537937bc46b5d6ff4ecc8",
    "439fc29ffa6a53fa0c72712467f98bb2ad2f5694c74a5c8ccda09c3a10dcb7b02c1df4320317a51098ac79cd31de52bf08637d67f738c602ac96f6123fcbdb7a",
    "69f59b9d4634f587e4259388895b883807d7cdc1ee6f7a00d1954e8ea38feed82bf3421de30fc585d0f6433003edb1a4914a74a653c830c32ce93fb9a3d5ed3b",
    "6a81d44cc8c9abe60df544cb3a57dd7a1881ff84ab73fb90594aad58b147b73d80697924191930d574964c2010680317a58c81eed833a9ba8046e1d591c0026c",
    "cbf1845813835138a605009506ef53edda114830ee4e13e3a579c6eb90f2658a5bfa1226d7572d37ac2e5cc95b60755dc5f09634bd8e3c68f55c354cea1a04e5",
    "52e2601f3e024544a6190c9717b356fe6883cd883a9fea4e86f1c5e5ec891327c23bb67472896b5b668ab4eb69eeb73fd702ac5d441fe312b8d00cd7185ccedb",
    "a43e6001dfc7af5594dcf8b7db2d7fab8b77786b834f0d8511351a5a266b03d61df66113320493b5cca1b2771b1d309b757095cec55353c22fae68de1ee892b4",
    "0593c3729aeb441c36a73eba97a62c2a125c1ef38d1c32e714696d0b0db79c3fd0fdff37b7c87d0cfb412dce673283291cf18e63fc9df67ada94946f925d5746"
  };

  [TestMethod]
  public void HashTests() {
    
    // Ensure all arrays are equal length.
    Assert.AreEqual(TestStrings.Length, ExpectedMD5s.Length);
    Assert.AreEqual(TestStrings.Length, ExpectedSHA1s.Length);
    Assert.AreEqual(TestStrings.Length, ExpectedSHA256s.Length);
    Assert.AreEqual(TestStrings.Length, ExpectedSHA512s.Length);
    
    // Cover DefaultEncoding
    Assert.AreEqual(MD5.DefaultEncoding, Encoding.UTF8);
    Assert.AreEqual(SHA1.DefaultEncoding, Encoding.UTF8);
    Assert.AreEqual(SHA256.DefaultEncoding, Encoding.UTF8);
    Assert.AreEqual(SHA512.DefaultEncoding, Encoding.UTF8);

    for (int i = 0; i < TestStrings.Length; i++) {

      Assert.AreEqual(ExpectedMD5s[i], MD5.GetMD5(TestStrings[i]).ToLower()); // MD5
      Assert.AreEqual(ExpectedSHA1s[i], SHA1.GetSHA1(TestStrings[i]).ToLower()); // SHA1
      Assert.AreEqual(ExpectedSHA256s[i], SHA256.GetSHA256(TestStrings[i]).ToLower()); // SHA256
      Assert.AreEqual(ExpectedSHA512s[i], SHA512.GetSHA512(TestStrings[i]).ToLower()); // SHA512

    }
    
  }
  
}