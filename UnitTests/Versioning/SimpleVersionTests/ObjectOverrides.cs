using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

public class ObjectOverrides
{
#region ToString

    [Fact]
    public void SimpleVersion_ToString_ReturnsCorrectVersionString()
    {
        // Arrange
        var version = new SimpleVersion(new uint[] { 1, 2, 3 });
        string expected = "1.2.3";

        // Act
        string actual = version.ToString();

        // Assert
        Assert.Equal(expected, actual);
    }

#endregion

#region Equals

    [Fact]
    public void SimpleVersion_Equals_ReturnsTrueForEqualVersions()
    {
        // Arrange
        var version1 = new SimpleVersion(new uint[] { 1, 2, 3 });
        var version2 = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        bool actual = version1.Equals(version2);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public void SimpleVersion_Equals_ReturnsFalseForUnequalVersions()
    {
        // Arrange
        var version1 = new SimpleVersion(new uint[] { 1, 2, 3 });
        var version2 = new SimpleVersion(new uint[] { 3, 2, 1 });

        // Act
        bool actual = version1.Equals(version2);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void SimpleVersion_Equals_ReturnsFalseForNull()
    {
        // Arrange
        var version = new SimpleVersion(new uint[] { 1, 2, 3 });
        SimpleVersion? nullVersion = null;

        // Act
        bool actual = version.Equals(nullVersion);

        // Assert
        Assert.False(actual);
    }

    [Fact]
    public void SimpleVersion_Equals_ReturnsFalseForOtherObject()
    {
        // Arrange
        var version = new SimpleVersion(new uint[] { 1, 2, 3 });
        var otherObject = new object();

        // Act
        bool actual = version.Equals(otherObject);

        // Assert
        Assert.False(actual);
    }

#endregion

#region GetHashCode

    [Fact]
    public void SimpleVersion_GetHashCode_ReturnsCorrectHashCode()
    {
        // Arrange
        var version = new SimpleVersion(new uint[] { 1, 2, 3 });
        string expectedCSV = "1,2,3";
        int expected = expectedCSV.GetHashCode();

        // Act
        int actual = version.GetHashCode();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void SimpleVersion_GetHashCode_ReturnsSameHashCodeForEqualVersions()
    {
        // Arrange
        var version1 = new SimpleVersion(new uint[] { 1, 2, 3 });
        var version2 = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        int version1Hash = version1.GetHashCode();
        int version2Hash = version2.GetHashCode();

        // Assert
        Assert.True(version1Hash == version2Hash);
    }

    [Fact]
    public void SimpleVersion_GetHashCode_ReturnsDifferentHashCodeForUnequalVersions()
    {
        // Arrange
        var version1 = new SimpleVersion(new uint[] { 1, 2, 3 });
        var version2 = new SimpleVersion(new uint[] { 3, 2, 1 });

        // Act
        int version1Hash = version1.GetHashCode();
        int version2Hash = version2.GetHashCode();

        // Assert
        Assert.True(version1Hash != version2Hash);
    }

#endregion
}