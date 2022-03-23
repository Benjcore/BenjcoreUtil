using Crypt = System.Security.Cryptography;
using System.Text;

namespace BenjcoreUtil.Security.Hashing; 

/// <summary>
/// Provides simple SHA1 hashing functions.
/// </summary>
public static class SHA1 {
  
  /// <summary>
  /// Default text encoding to use if none is specified.
  /// </summary>
  public static Encoding DefaultEncoding = Encoding.UTF8;

  /// <summary>
  /// Gets the SHA1 Hash of the given input.
  /// </summary>
  /// <param name="input">A string to hash.</param>
  /// <param name="encoding">The encoding of input. If encoding is unspecified or null, 
  /// the DefaultEncoding field will be used instead. (UTF-8 by default.)</param>
  /// <returns>The SHA1 hash of the input.</returns>
  public static string GetSHA1(string input, Encoding? encoding = null) {
    
    // Sets encoding to the value of DefaultEncoding if the encoding isn't specified.
    encoding ??= DefaultEncoding;

    byte[] inputBin = encoding.GetBytes(input);
    return GetSHA1(inputBin);
    
  }

  /// <summary>
  /// Gets the SHA1 Hash of the given input.
  /// </summary>
  /// <param name="input">A byte array to hash.</param>
  /// <returns>The SHA1 hash of the input.</returns>
  public static string GetSHA1(byte[] input) {

    using (Crypt.SHA1 sha1 = Crypt.SHA1.Create()) {
      
      byte[] binResult = sha1.ComputeHash(input);
      StringBuilder sb = new StringBuilder();
      foreach (byte item in binResult) {
        sb.Append(item.ToString("X2"));
      }
      return sb.ToString();
      
    }
    
  }
  
  
}