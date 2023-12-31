using Crypt = System.Security.Cryptography;
using System.Text;

namespace BenjcoreUtil.Security.Hashing;

/// <summary>
/// Provides simple MD5 hashing functions.
/// </summary>
public static class MD5
{
    /// <summary>
    /// Default text encoding to use if none is specified.
    /// </summary>
    public static Encoding DefaultEncoding = Encoding.UTF8;
    
    /// <summary>
    /// Gets the MD5 Hash of the given input.
    /// </summary>
    /// <param name="input">A string to hash.</param>
    /// <param name="encoding">The encoding of input. If encoding is unspecified or null, 
    /// the DefaultEncoding field will be used instead. (UTF-8 by default.)</param>
    /// <returns>The MD5 hash of the input.</returns>
    public static string GetMD5(string input, Encoding? encoding = null)
    {
        // Sets encoding to the value of DefaultEncoding if the encoding isn't specified.
        encoding ??= DefaultEncoding;
        
        byte[] inputBin = encoding.GetBytes(input);
        return GetMD5(inputBin);
    }
    
    /// <summary>
    /// Gets the MD5 Hash of the given input.
    /// </summary>
    /// <param name="input">A byte array to hash.</param>
    /// <returns>The MD5 hash of the input.</returns>
    public static string GetMD5(byte[] input)
    {
        using (Crypt.MD5 md5 = Crypt.MD5.Create())
        {
            byte[] binResult = md5.ComputeHash(input);
            StringBuilder sb = new StringBuilder();
            
            foreach (byte item in binResult)
            {
                sb.Append(item.ToString("X2"));
            }
            
            return sb.ToString();
        }
    }
}