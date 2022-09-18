using System.Diagnostics.CodeAnalysis;
using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

public class MiscTests
{
    // This file contains miscellaneous SimpleVersion tests that don't fit anywhere else.
    
    [Fact]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    [SuppressMessage("Performance", "CA1806:Do not ignore method results")]
    public void SimpleVersion_Constructor_ThrowsOnEmptyArray()
    {
        // Arrange
        uint[] emptyArray = Array.Empty<uint>();
        
        // Act
        Action action = () => new SimpleVersion(emptyArray);
        
        // Assert
        Assert.Throws<ArgumentException>(action);
    }
    
    [Fact]
    public void SimpleVersion_ExplicitStringConversion_ReturnsCorrectValue()
    {
        // Arrange
        SimpleVersion version = new SimpleVersion(new uint[] { 1, 2, 3 });
        string expected = version.ToString();

        // Act
        string actual = (string)version;                   // Explicit conversion to string.
        var actualAsVersion = SimpleVersion.Parse(actual); // Parse the string back into a SimpleVersion.

        // Assert
        Assert.Equal(expected, actual);                  // Test the explicit conversion to string.
        Assert.True(version.IsEqualTo(actualAsVersion)); // Test the explicit conversion to string
                                                         // by re-parsing as a SimpleVersion.
    }
}