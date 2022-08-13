using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BenjcoreUtil.Security.Encryption;

/// <summary>
/// An object representing a pair of RSA keys.
/// </summary>
public class RSAKeys
{
    /// <summary>
    /// Use OAEP if true, else use PKCS#1.5.
    /// </summary>
    protected const bool OAEP = true;

    /// <summary>
    /// The RSA default key size.
    /// </summary>
    private const RSAKeyLengths DefaultKeySize = RSAKeyLengths.RSA2048;

    /// <summary>
    /// Returns an integer representation of <see cref="UseKeySize"/>.
    /// </summary>
    /// <exception cref="InvalidEnumArgumentException"><see cref="UseKeySize"/> is an invalid value.</exception>
    protected int KeySize
    {
        get
        {
            return UseKeySize switch
            {
                RSAKeyLengths.RSA2048 => 2048,
                RSAKeyLengths.RSA4096 => 4096,
                _ => throw new InvalidEnumArgumentException($"{nameof(UseKeySize)} is an invalid value.")
            };
        }
    }

    /// <summary>
    /// A <see cref="RSAKeyLengths"/> field which dictates the RSA key length used by the <see cref="RSAKeys"/> object.
    /// </summary>
    /// <remarks>This field is readonly.</remarks>
    public readonly RSAKeyLengths UseKeySize;

    /// <summary>
    /// A <see cref="RSAParameters"/> object representing the RSA private key of the <see cref="RSAKeys"/> object.
    /// </summary>
    public RSAParameters PrivateKey { get; private set; }

    /// <summary>
    /// Returns a string representation of <see cref="PrivateKey"/>.
    /// </summary>
    public string PrivateKeyString => KeyToString(PrivateKey);

    /// <summary>
    /// A <see cref="RSAParameters"/> object representing the RSA public key of the <see cref="RSAKeys"/> object.
    /// </summary>
    public RSAParameters PublicKey { get; private set; }

    /// <summary>
    /// Returns a string representation of <see cref="PublicKey"/>.
    /// </summary>
    public string PublicKeyString => KeyToString(PublicKey);

    /// <summary>
    /// Creates a new instance of the <see cref="RSAKeys"/> class.
    /// </summary>
    /// <param name="keyLengths">A <see cref="RSAKeyLengths"/> object
    /// to dictate the RSA key length of the new object.</param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="keyLengths"/> is an invalid value.</exception>
    /// <remarks>
    /// This constructor calls <see cref="UpdateKeys"/> which is performance intensive,
    /// if you find this to be an issue you can use the asynchronous version <see cref="CreateAsync"/>.
    /// </remarks>
    public RSAKeys(RSAKeyLengths keyLengths = DefaultKeySize)
    {
        UseKeySize = keyLengths;

        try
        {
            UpdateKeys();
        }
        catch (InvalidEnumArgumentException e)
        {
            throw new InvalidEnumArgumentException($"Invalid argument value of {nameof(keyLengths)}.", e);
        }
    }

    /// <summary>
    /// Asynchronously creates a new instance of the <see cref="RSAKeys"/> class.
    /// </summary>
    /// <param name="keyLengths">A <see cref="RSAKeyLengths"/> object
    /// to dictate the RSA key length of the new object.</param>
    /// <exception cref="InvalidEnumArgumentException"><paramref name="keyLengths"/> is an invalid value.</exception>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public static async Task<RSAKeys> CreateAsync(RSAKeyLengths keyLengths = DefaultKeySize)
    {
        var task = Task.Run(() => new RSAKeys(keyLengths));
        return await task;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="RSAKeys"/> class
    /// by cloning an existing <see cref="RSAKeys"/> object.
    /// </summary>
    /// <param name="objectToClone">The <see cref="RSAKeys"/> object to clone.</param>
    /// <exception cref="InvalidEnumArgumentException">The parameter keyLengths in
    /// <paramref name="objectToClone"/> is an invalid value.</exception>
    /// <remarks>
    /// The object is cloned by copying the following fields / properties:
    /// <p>
    ///   <see cref="UseKeySize"/><br></br>
    ///   <see cref="PublicKey"/><br></br>
    ///   <see cref="PrivateKey"/>
    /// </p>
    /// </remarks>
    public RSAKeys(RSAKeys objectToClone)
    {
        UseKeySize = objectToClone.UseKeySize;
        PublicKey = objectToClone.PublicKey;
        PrivateKey = objectToClone.PrivateKey;
    }

    /// <summary>
    /// Generate a new RSA keypair which will override <see cref="PublicKey"/> and <see cref="PrivateKey"/>.
    /// </summary>
    /// <remarks>
    /// This is performance intensive, if you find this to be an issue you
    /// can use the asynchronous version <see cref="UpdateKeysAsync"/>.
    /// </remarks>
    public void UpdateKeys()
    {
        // Create a RSA-CSP
        var csp = new RSACryptoServiceProvider(KeySize);

        // Get Keys
        var privateKey = csp.ExportParameters(true); // Private
        var publicKey = csp.ExportParameters(false); // Public

        // Update Keys
        PrivateKey = privateKey;
        PublicKey = publicKey;
    }

    /// <summary>
    /// Asynchronously calls <see cref="UpdateKeys"/>.
    /// </summary>
    public async Task UpdateKeysAsync()
    {
        var task = Task.Run(UpdateKeys);
        await task;
    }

    /// <summary>
    /// Coverts a <see cref="RSAParameters"/> key to a byte array.
    /// </summary>
    /// <param name="key">The <see cref="RSAParameters"/> key you wish to convert.</param>
    /// <returns>A byte array representing <paramref name="key"/>.</returns>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static byte[] DecodeKey(RSAParameters key)
    {
        var sw = new Utf8StringWriter();
        var wrt = XmlWriter.Create(sw);
        var xs = new XmlSerializer(typeof(RSAParameters));
        xs.Serialize(wrt, key);
        return Encoding.UTF8.GetBytes(sw.ToString());
    }

    /// <summary>
    /// Coverts a <see cref="RSAParameters"/> key to a string.
    /// </summary>
    /// <param name="key">The <see cref="RSAParameters"/> key you wish to convert.</param>
    /// <returns>A string representation of <paramref name="key"/>.</returns>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static string KeyToString(RSAParameters key)
    {
        return Encoding.UTF8.GetString(DecodeKey(key));
    }

    /// <summary>
    /// Parses a <see cref="RSAParameters"/> object from a string parameter.
    /// </summary>
    /// <param name="input">The string you wish to convert to a <see cref="RSAParameters"/> object.</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Invalid XML in <paramref name="input"/>.</exception>
    /// <exception cref="NullReferenceException">Conversion returned null.</exception>
    public static RSAParameters GetKey(string input)
    {
        var sr = new StringReader(input);
        var xmlReader = new XmlTextReader(sr) { Normalization = false };
        var xs = new XmlSerializer(typeof(RSAParameters));
        return (RSAParameters) (xs.Deserialize(xmlReader) ?? throw new NullReferenceException());
    }

    //
    // Encrypt
    //

    /// <summary>
    /// Encrypts data using a RSA public key.
    /// </summary>
    /// <param name="input">A string containing the data you wish to encrypt.</param>
    /// <param name="publicKey">The public key that will be used to encrypt <paramref name="input"/>.</param>
    /// <returns>A byte array containing the encrypted data.</returns>
    /// <exception cref="CryptographicException"><paramref name="publicKey"/> is invalid.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static byte[] Encrypt(string input, RSAParameters publicKey)
    {
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(publicKey);
        var inputBin = Encoding.UTF8.GetBytes(input);
        return csp.Encrypt(inputBin, OAEP);
    }

    /// <summary>
    /// Encrypts data using a RSA public key.
    /// </summary>
    /// <param name="input">A byte array containing the data you wish to encrypt.</param>
    /// <param name="publicKey">The public key that will be used to encrypt <paramref name="input"/>.</param>
    /// <returns>A byte array containing the encrypted data.</returns>
    /// <exception cref="CryptographicException"><paramref name="publicKey"/> is invalid.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static byte[] Encrypt(byte[] input, RSAParameters publicKey)
    {
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(publicKey);
        return csp.Encrypt(input, OAEP);
    }

    /// <summary>
    /// Encrypts data using a RSA public key and encodes it using base64.
    /// </summary>
    /// <param name="input">A string containing the data you wish to encrypt.</param>
    /// <param name="publicKey">The public key that will be used to encrypt <paramref name="input"/>.</param>
    /// <returns>A base64 encoded string containing the encrypted data.</returns>
    /// <exception cref="CryptographicException"><paramref name="publicKey"/> is invalid.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static string EncryptBase64(string input, RSAParameters publicKey)
    {
        var output = Encrypt(input, publicKey);
        return Convert.ToBase64String(output);
    }

    /// <summary>
    /// Encrypts data using a RSA public key and encodes it using base64.
    /// </summary>
    /// <param name="input">A byte array containing the data you wish to encrypt.</param>
    /// <param name="publicKey">The public key that will be used to encrypt <paramref name="input"/>.</param>
    /// <returns>A base64 encoded string containing the encrypted data.</returns>
    /// <exception cref="CryptographicException"><paramref name="publicKey"/> is invalid.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static string EncryptBase64(byte[] input, RSAParameters publicKey)
    {
        var output = Encrypt(input, publicKey);
        return Convert.ToBase64String(output);
    }

    //
    // Decrypt
    //

    /// <summary>
    /// Decrypts data using a RSA private key.
    /// </summary>
    /// <param name="base64Input">A string containing the encrypted data encoded in base64.</param>
    /// <param name="privateKey">The private key that will be used to decrypt <paramref name="base64Input"/>.</param>
    /// <returns>A string containing the decrypted result.</returns>
    /// <exception cref="CryptographicException">Decryption failed.</exception>
    /// <exception cref="FormatException"><paramref name="base64Input"/> is not valid base64.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static string Decrypt(string base64Input, RSAParameters privateKey)
    {
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(privateKey);
        var inputBin = Convert.FromBase64String(base64Input);
        var outputBin = csp.Decrypt(inputBin, OAEP);
        return Encoding.UTF8.GetString(outputBin);
    }

    /// <summary>
    /// Decrypts data using a RSA private key.
    /// </summary>
    /// <param name="inputBytes">A byte array containing the encrypted data.</param>
    /// <param name="privateKey">The private key that will be used to decrypt <paramref name="inputBytes"/>.</param>
    /// <returns>A string containing the decrypted result.</returns>
    /// <exception cref="CryptographicException">Decryption failed.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static string Decrypt(byte[] inputBytes, RSAParameters privateKey)
    {
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(privateKey);
        var outputBin = csp.Decrypt(inputBytes, OAEP);
        return Encoding.UTF8.GetString(outputBin);
    }

    /// <summary>
    /// Decrypts data using a RSA private key.
    /// </summary>
    /// <param name="base64Input">A string containing the encrypted data encoded in base64.</param>
    /// <param name="privateKey">The private key that will be used to decrypt <paramref name="base64Input"/>.</param>
    /// <returns>A byte array containing the decrypted result.</returns>
    /// <exception cref="CryptographicException">Decryption failed.</exception>
    /// <exception cref="FormatException"><paramref name="base64Input"/> is not valid base64.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static byte[] DecryptBytes(string base64Input, RSAParameters privateKey)
    {
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(privateKey);
        var inputBin = Convert.FromBase64String(base64Input);
        return csp.Decrypt(inputBin, OAEP);
    }

    /// <summary>
    /// Decrypts data using a RSA private key.
    /// </summary>
    /// <param name="inputBytes">A byte array containing the encrypted data.</param>
    /// <param name="privateKey">The private key that will be used to decrypt <paramref name="inputBytes"/>.</param>
    /// <returns>A byte array containing the decrypted result.</returns>
    /// <exception cref="CryptographicException">Decryption failed.</exception>
    /// <remarks>This method uses UTF-8 encoding.</remarks>
    public static byte[] DecryptBytes(byte[] inputBytes, RSAParameters privateKey)
    {
        var csp = new RSACryptoServiceProvider();
        csp.ImportParameters(privateKey);
        return csp.Decrypt(inputBytes, OAEP);
    }

    /// <summary>
    /// Determines whether this instance and another specified RSAKeys object have the same value.
    /// </summary>
    /// <param name="rsaKeys">The RSAKeys object to compare to this instance.</param>
    /// <returns>true if the value of the value parameter is the same as the value of this instance;
    /// otherwise, false. If value is null, the method returns false.</returns>
    public bool Equals(RSAKeys? rsaKeys)
    {
        if
        (
            rsaKeys != null &&
            UseKeySize == rsaKeys.UseKeySize &&
            PublicKeyString.Equals(rsaKeys.PublicKeyString) &&
            PrivateKeyString.Equals(rsaKeys.PrivateKeyString)
        )
        {
            return true;
        }

        return false;
    }
}

/// <summary>
/// An enum that represents a RSA key length. e.g.
/// <p>RSA2048 or RSA4096.</p>
/// </summary>
public enum RSAKeyLengths
{
    RSA2048,
    RSA4096
}

/// <summary>
/// StringWriter override that uses UTF-8 rather than UTF-16.
/// </summary>
internal class Utf8StringWriter : StringWriter
{
    public override Encoding Encoding => Encoding.UTF8;
}