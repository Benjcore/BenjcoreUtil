using System.Text;
using BenjcoreUtil.Security.Hashing;

// ReSharper disable StringLiteralTypo

namespace UnitTests.Security.Hashing;

public class HashingTests
{
    private const string SampleText = 
        "The quick brown fox jumps over the lazy dog. The quick brown fox jumps over the lazy dog. 1234567890";
    
    private static readonly byte[] SampleBytes = Encoding.UTF8.GetBytes(SampleText);
    
    #region MD5
    
    [Fact]
    public void MD5_ReturnsMD5HashGivenString()
    {
        // Arrange
        const string expected = "39847c580e6761548747a18b7d82202d";
        
        // Act
        string actual = MD5.GetMD5(SampleText).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void MD5_ReturnsMD5HashGivenBytes()
    {
        // Arrange
        const string expected = "39847c580e6761548747a18b7d82202d";
        
        // Act
        string actual = MD5.GetMD5(SampleBytes).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void MD5_ReturnsMD5HashGivenBytesUTF8()
    {
        // Arrange
        const string expected = "39847c580e6761548747a18b7d82202d";
        
        // Act
        string actual = MD5.GetMD5(SampleText, Encoding.UTF8).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    #endregion
    
    #region SHA1
    
    [Fact]
    public void SHA1_ReturnsSHA1HashGivenString()
    {
        // Arrange
        const string expected = "80e970c71526f57c8fb4585b1f015dc5e67ccc45";
        
        // Act
        string actual = SHA1.GetSHA1(SampleText).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void SHA1_ReturnsSHA1HashGivenBytes()
    {
        // Arrange
        const string expected = "80e970c71526f57c8fb4585b1f015dc5e67ccc45";
        
        // Act
        string actual = SHA1.GetSHA1(SampleBytes).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void SHA1_ReturnsSHA1HashGivenBytesUTF8()
    {
        // Arrange
        const string expected = "80e970c71526f57c8fb4585b1f015dc5e67ccc45";
        
        // Act
        string actual = SHA1.GetSHA1(SampleText, Encoding.UTF8).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    #endregion
    
    #region SHA256
    
    [Fact]
    public void SHA256_ReturnsSHA256HashGivenString()
    {
        // Arrange
        const string expected = "d73b6ed72ec8e3b77c8cab7358af99031e352564f5b60fb6beef9027f293448e";
        
        // Act
        string actual = SHA256.GetSHA256(SampleText).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void SHA256_ReturnsSHA256HashGivenBytes()
    {
        // Arrange
        const string expected = "d73b6ed72ec8e3b77c8cab7358af99031e352564f5b60fb6beef9027f293448e";
        
        // Act
        string actual = SHA256.GetSHA256(SampleBytes).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void SHA256_ReturnsSHA256HashGivenBytesUTF8()
    {
        // Arrange
        const string expected = "d73b6ed72ec8e3b77c8cab7358af99031e352564f5b60fb6beef9027f293448e";
        
        // Act
        string actual = SHA256.GetSHA256(SampleText, Encoding.UTF8).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    #endregion
    
    #region SHA512
    
    [Fact]
    public void SHA512_ReturnsSHA512HashGivenString()
    {
        // Arrange
        const string expected =
            "eb496528f6d04039426921dd3df5668a2d0eab8187b4d051938b29dd42f9704e" +
            "a2a3b0a336d29c8cff77a986db02ff9cc8c6ca869ec3729e50af316783447498";
        
        // Act
        string actual = SHA512.GetSHA512(SampleText).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void SHA512_ReturnsSHA512HashGivenBytes()
    {
        // Arrange
        const string expected =
            "eb496528f6d04039426921dd3df5668a2d0eab8187b4d051938b29dd42f9704e" +
            "a2a3b0a336d29c8cff77a986db02ff9cc8c6ca869ec3729e50af316783447498";
        
        // Act
        string actual = SHA512.GetSHA512(SampleBytes).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void SHA512_ReturnsSHA512HashGivenBytesUTF8()
    {
        // Arrange
        const string expected =
            "eb496528f6d04039426921dd3df5668a2d0eab8187b4d051938b29dd42f9704e" +
            "a2a3b0a336d29c8cff77a986db02ff9cc8c6ca869ec3729e50af316783447498";
        
        // Act
        string actual = SHA512.GetSHA512(SampleText, Encoding.UTF8).ToLower();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    #endregion
}