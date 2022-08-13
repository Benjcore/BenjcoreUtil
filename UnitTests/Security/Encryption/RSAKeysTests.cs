using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BenjcoreUtil.Security.Encryption;

namespace UnitTests.Security.Encryption;

public class RSAKeysTests
{
    private const string TestString =
        "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 1234567890";

    private readonly byte[] TestBytes = Encoding.UTF8.GetBytes(TestString);

    /*
     * These unit tests are hella slow. This is due
     * to the large amount of RSA key generation.
     */

#region RSA2048

    [Fact]
    public async void RSAKeys_2048_EncryptDecrypt_CreateAsync()
    {
        // Arrange
        var rsaKeys = await RSAKeys.CreateAsync(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public async void RSAKeys_2048_CreateAsync_ThrowsOnInvalidEnum()
    {
        // Arrange
        var invalidEnum = (RSAKeyLengths) 0xFF;

        // Act
        var function = new Func<Task<RSAKeys>>(async () => await RSAKeys.CreateAsync(invalidEnum));

        // Assert
        await Assert.ThrowsAsync<InvalidEnumArgumentException>(function);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecryptWithCloningConstructor()
    {
        // Arrange
        var rsaKeys1 = new RSAKeys(RSAKeyLengths.RSA2048);
        // ReSharper disable once JoinDeclarationAndInitializer
        RSAKeys rsaKeys2;

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys1.PublicKey);
        rsaKeys2 = new RSAKeys(rsaKeys1);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys2.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecryptThrowsOnWrongKey()
    {
        // Arrange
        var rightKeys = new RSAKeys(RSAKeyLengths.RSA2048);
        var wrongKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rightKeys.PublicKey);

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, wrongKeys.PrivateKey));
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecryptThrowsOnDecryptingRSA4096()
    {
        // Arrange
        var rightKeys = new RSAKeys(RSAKeyLengths.RSA4096);
        var wrongKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rightKeys.PublicKey);

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, wrongKeys.PrivateKey));
    }

    [Fact]
    public void RSAKeys_2048_UpdateKeysChangesKeys()
    {
        // Arrange
        var keys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, keys.PublicKey);
        keys.UpdateKeys();

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, keys.PrivateKey));
    }

    [Fact]
    public async void RSAKeys_2048_UpdateKeysAsyncChangesKeys()
    {
        // Arrange
        var keys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, keys.PublicKey);
        await keys.UpdateKeysAsync();

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, keys.PrivateKey));
    }

    [Fact]
    public void RSAKeys_2048_EqualsReturnsTrueWhenEqual()
    {
        // Arrange
        var keys1 = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        var keys2 = new RSAKeys(keys1);

        // Assert
        Assert.True(keys1.Equals(keys1));
        Assert.True(keys1.Equals(keys2));
    }

    [Fact]
    public void RSAKeys_2048_EqualsReturnsFalseWhenNotEqual()
    {
        // Arrange
        var keys1 = new RSAKeys(RSAKeyLengths.RSA2048);
        var keys2 = new RSAKeys(keys1);
        keys2.UpdateKeys();

        // Act
        var keys3 = new RSAKeys(RSAKeyLengths.RSA2048);

        // Assert
        Assert.False(keys1.Equals(keys2));
        Assert.False(keys1.Equals(keys3));
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_GetKey()
    {
        // Arrange
        var keys = new RSAKeys(RSAKeyLengths.RSA2048);
        string publicKeyString = keys.PublicKeyString;
        string privateKeyString = keys.PrivateKeyString;

        // Act
        var publicKey = RSAKeys.GetKey(publicKeyString);
        var privateKey = RSAKeys.GetKey(privateKeyString);
        var encrypted = RSAKeys.Encrypt(TestString, publicKey);
        var decrypted = RSAKeys.Decrypt(encrypted, privateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

#region RSA2048 Encrypt Decrypt Combinations

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_BytesToBytesToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_BytesToBytesToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_StringToBytesToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_StringToBytesToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_BytesToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_BytesToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_StringToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecrypt_StringToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

#endregion

#region RSA2048 Encrypt Decrypt Base64 Combinations

    [Fact]
    public void RSAKeys_2048_EncryptDecryptBase64_BytesToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestBytes, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecryptBase64_StringToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecryptBase64_StringToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestString, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_2048_EncryptDecryptBase64_BytesToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA2048);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestBytes, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

#endregion

#endregion

#region RSA4096

    [Fact]
    public async void RSAKeys_4096_EncryptDecrypt_CreateAsync()
    {
        // Arrange
        var rsaKeys = await RSAKeys.CreateAsync(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public async void RSAKeys_4096_CreateAsync_ThrowsOnInvalidEnum()
    {
        // Arrange
        var invalidEnum = (RSAKeyLengths) 0xFF;

        // Act
        var function = new Func<Task<RSAKeys>>(async () => await RSAKeys.CreateAsync(invalidEnum));

        // Assert
        await Assert.ThrowsAsync<InvalidEnumArgumentException>(function);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecryptWithCloningConstructor()
    {
        // Arrange
        var rsaKeys1 = new RSAKeys(RSAKeyLengths.RSA4096);
        // ReSharper disable once JoinDeclarationAndInitializer
        RSAKeys rsaKeys2;

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys1.PublicKey);
        rsaKeys2 = new RSAKeys(rsaKeys1);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys2.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecryptThrowsOnWrongKey()
    {
        // Arrange
        var rightKeys = new RSAKeys(RSAKeyLengths.RSA4096);
        var wrongKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rightKeys.PublicKey);

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, wrongKeys.PrivateKey));
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecryptThrowsOnDecryptingRSA2048()
    {
        // Arrange
        var rightKeys = new RSAKeys(RSAKeyLengths.RSA2048);
        var wrongKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rightKeys.PublicKey);

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, wrongKeys.PrivateKey));
    }

    [Fact]
    public void RSAKeys_4096_UpdateKeysChangesKeys()
    {
        // Arrange
        var keys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, keys.PublicKey);
        keys.UpdateKeys();

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, keys.PrivateKey));
    }

    [Fact]
    public async void RSAKeys_4096_UpdateKeysAsyncChangesKeys()
    {
        // Arrange
        var keys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, keys.PublicKey);
        await keys.UpdateKeysAsync();

        // Assert
        Assert.Throws<CryptographicException>(() => RSAKeys.Decrypt(encrypted, keys.PrivateKey));
    }

    [Fact]
    public void RSAKeys_4096_EqualsReturnsTrueWhenEqual()
    {
        // Arrange
        var keys1 = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        var keys2 = new RSAKeys(keys1);

        // Assert
        Assert.True(keys1.Equals(keys1));
        Assert.True(keys1.Equals(keys2));
    }

    [Fact]
    public void RSAKeys_4096_EqualsReturnsFalseWhenNotEqual()
    {
        // Arrange
        var keys1 = new RSAKeys(RSAKeyLengths.RSA4096);
        var keys2 = new RSAKeys(keys1);
        keys2.UpdateKeys();

        // Act
        var keys3 = new RSAKeys(RSAKeyLengths.RSA4096);

        // Assert
        Assert.False(keys1.Equals(keys2));
        Assert.False(keys1.Equals(keys3));
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_GetKey()
    {
        // Arrange
        var keys = new RSAKeys(RSAKeyLengths.RSA4096);
        string publicKeyString = keys.PublicKeyString;
        string privateKeyString = keys.PrivateKeyString;

        // Act
        var publicKey = RSAKeys.GetKey(publicKeyString);
        var privateKey = RSAKeys.GetKey(privateKeyString);
        var encrypted = RSAKeys.Encrypt(TestString, publicKey);
        var decrypted = RSAKeys.Decrypt(encrypted, privateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

#region RSA4096 Encrypt Decrypt Combinations

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_BytesToBytesToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_BytesToBytesToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_StringToBytesToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_StringToBytesToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_BytesToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_BytesToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestBytes, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_StringToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecrypt_StringToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        byte[] encrypted = RSAKeys.Encrypt(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

#endregion

#region RSA4096 Encrypt Decrypt Base64 Combinations

    [Fact]
    public void RSAKeys_4096_EncryptDecryptBase64_BytesToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestBytes, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecryptBase64_StringToStringToString()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestString, rsaKeys.PublicKey);
        string decrypted = RSAKeys.Decrypt(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestString, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecryptBase64_StringToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestString, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

    [Fact]
    public void RSAKeys_4096_EncryptDecryptBase64_BytesToStringToBytes()
    {
        // Arrange
        var rsaKeys = new RSAKeys(RSAKeyLengths.RSA4096);

        // Act
        string encrypted = RSAKeys.EncryptBase64(TestBytes, rsaKeys.PublicKey);
        byte[] decrypted = RSAKeys.DecryptBytes(encrypted, rsaKeys.PrivateKey);

        // Assert
        Assert.Equal(TestBytes, decrypted);
    }

#endregion

#endregion

    [Fact]
    public void Utf8StringWriter()
    {
        // Arrange
        using var writer = new Utf8StringWriter();
        byte[] expected = Encoding.UTF8.GetBytes(TestString);

        // Act
        writer.Write(TestString);
        writer.Flush();
        byte[] actual = Encoding.UTF8.GetBytes(writer.ToString());

        // Assert
        Assert.Equal(expected, actual); // Check that the Utf8StringWriter's string is UTF8.
        Assert.Equal(Encoding.UTF8, writer.Encoding); // Check that the Utf8StringWriter's encoding is UTF8.
    }
}