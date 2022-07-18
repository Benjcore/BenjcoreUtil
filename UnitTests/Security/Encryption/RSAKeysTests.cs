// ReSharper disable RedundantUsingDirective
// ^ Because of a Rider issue.
using BenjcoreUtil.Security.Encryption;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests.Security.Encryption; 

[TestClass]
public class RSAKeysTests {

  public const string TestString =
    "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 1234567890";

  [TestMethod]
  public void RSAKeysTest() {
    
    // Cover KeySize ArgumentNullException
    {
      try {
        var dummy = new RSAKeys(keyLengths:(RSAKeyLengths)UInt16.MaxValue);
        Assert.IsTrue(false);
      } catch (InvalidEnumArgumentException) {
        Assert.IsTrue(true);
      }
    }

    {
      var task = RSAKeys.CreateAsync();
      while (!task.IsCompleted) { }
      Assert.IsFalse(String.IsNullOrEmpty(task.Result.PublicKeyString));
    }

    bool firstPass = true;

    RSAKeys[] kpa = {
      new (),
      new (RSAKeyLengths.RSA4096)
    };
    
    top:

    foreach (var kp in kpa) {

      Assert.IsFalse(String.IsNullOrEmpty(kp.PublicKeyString));
      Assert.IsFalse(String.IsNullOrEmpty(kp.PrivateKeyString));

      Assert.IsNotNull(kp.PublicKey);
      Assert.IsNotNull(kp.PrivateKey);

      Assert.IsTrue(kp.Equals(new RSAKeys(kp)));

      {
        RSAKeys kp2 = new RSAKeys(kp.UseKeySize);

        EnsureDifferent(kp, kp2);
        RSAKeys kp2Old = new RSAKeys(kp2);
        Assert.IsTrue(kp2.Equals(kp2Old));
        kp2.UpdateKeys();
        Assert.IsFalse(kp2.Equals(kp2Old));
      }

      {
        RSAKeys kp2 = new RSAKeys(kp);
        EnsureSame(kp, kp2);
        var task = kp2.UpdateKeysAsync();
        while (!task.IsCompleted) { }
        EnsureDifferent(kp, kp2);
      }

      // Validate Key Conversions
      Assert.AreEqual(kp.PublicKeyString, RSAKeys.KeyToString(kp.PublicKey));
      Assert.AreEqual(kp.PublicKeyString, RSAKeys.KeyToString(RSAKeys.GetKey(kp.PublicKeyString)));
      Assert.AreEqual(kp.PrivateKeyString, RSAKeys.KeyToString(kp.PrivateKey));
      Assert.AreEqual(kp.PrivateKeyString, RSAKeys.KeyToString(RSAKeys.GetKey(kp.PrivateKeyString)));
      
      // Encryption & Decryption Tests
      {
        RSAKeys kp2 = new RSAKeys(kp.UseKeySize);

        Assert.AreEqual(TestString, RSAKeys.Decrypt(RSAKeys.EncryptBase64(TestString, kp.PublicKey), kp.PrivateKey));
        Assert.AreEqual(TestString, RSAKeys.Decrypt(RSAKeys.EncryptBase64(Encoding.UTF8.GetBytes(TestString), kp.PublicKey), kp.PrivateKey));
        Assert.AreEqual(TestString, RSAKeys.Decrypt(Convert.FromBase64String(RSAKeys.EncryptBase64(TestString, kp.PublicKey)), kp.PrivateKey));
        Assert.AreEqual(TestString, Encoding.UTF8.GetString(RSAKeys.DecryptBytes(RSAKeys.EncryptBase64(Encoding.UTF8.GetBytes(TestString), kp.PublicKey), kp.PrivateKey)));
        Assert.AreEqual(TestString, Encoding.UTF8.GetString(RSAKeys.DecryptBytes(Convert.FromBase64String(RSAKeys.EncryptBase64(TestString, kp.PublicKey)), kp.PrivateKey)));

        try {
          Assert.AreNotEqual(TestString, RSAKeys.Decrypt(RSAKeys.EncryptBase64(TestString, kp.PublicKey), kp2.PrivateKey));
          Assert.AreNotEqual(TestString, RSAKeys.Decrypt(RSAKeys.EncryptBase64(Encoding.UTF8.GetBytes(TestString), kp.PublicKey), kp2.PrivateKey));
          Assert.AreNotEqual(TestString, RSAKeys.Decrypt(Convert.FromBase64String(RSAKeys.EncryptBase64(TestString, kp.PublicKey)), kp2.PrivateKey));
          Assert.AreNotEqual(TestString, Encoding.UTF8.GetString(RSAKeys.DecryptBytes(RSAKeys.EncryptBase64(Encoding.UTF8.GetBytes(TestString), kp.PublicKey), kp2.PrivateKey)));
          Assert.AreNotEqual(TestString, Encoding.UTF8.GetString(RSAKeys.DecryptBytes(Convert.FromBase64String(RSAKeys.EncryptBase64(TestString, kp.PublicKey)), kp2.PrivateKey)));
        } catch {
          Assert.IsTrue(true);
        }
      }
      
      // Update Keys
      kp.UpdateKeys();

    }
    
    // Try again after key updates.
    if (firstPass) {
      firstPass = false;
      goto top;
    }

  }

  public static void EnsureDifferent(RSAKeys in1, RSAKeys in2) {

    Assert.AreNotEqual(in1.PublicKey, in2.PublicKey);
    Assert.AreNotEqual(in1.PrivateKey, in2.PrivateKey);
    
    Assert.AreNotEqual(in1.PublicKeyString, in2.PublicKeyString);
    Assert.AreNotEqual(in1.PrivateKeyString, in2.PrivateKeyString);

  }

  public static void EnsureSame(RSAKeys in1, RSAKeys in2) {

    try {
      EnsureDifferent(in1, in2);
      Assert.IsTrue(false);
    } catch (AssertFailedException) {
      Assert.IsTrue(true);
    }
    
  }
  
}