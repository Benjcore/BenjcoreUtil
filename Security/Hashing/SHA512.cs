using Crypt = System.Security.Cryptography;
using System.Text;

namespace BenjcoreUtil.Security.Hashing;

/// <summary>
/// Provides simple SHA512 hashing functions.
/// </summary>
public static class SHA512
{
    /// <summary>
    /// Default text encoding to use if none is specified.
    /// </summary>
    public static Encoding DefaultEncoding = Encoding.UTF8;
    
    /// <summary>
    /// Gets the SHA512 Hash of the given input.
    /// </summary>
    /// <param name="input">A string to hash.</param>
    /// <param name="encoding">The encoding of input. If encoding is unspecified or null, 
    /// the DefaultEncoding field will be used instead. (UTF-8 by default.)</param>
    /// <returns>The SHA512 hash of the input.</returns>
    public static string GetSHA512(string input, Encoding? encoding = null)
    {
        // Sets encoding to the value of DefaultEncoding if the encoding isn't specified.
        encoding ??= DefaultEncoding;

        byte[] inputBin = encoding.GetBytes(input);
        return GetSHA512(inputBin);
    }
    
    /// <summary>
    /// Gets the SHA512 Hash of the given input.
    /// </summary>
    /// <param name="input">A byte array to hash.</param>
    /// <returns>The SHA512 hash of the input.</returns>
    public static string GetSHA512(byte[] input)
    {
        byte[] binResult = Crypt.SHA512.HashData(input);
        StringBuilder sb = new StringBuilder();
        
        foreach (byte item in binResult)
        {
            sb.Append(item.ToString("X2"));
        }
        
        return sb.ToString();
    }
}