using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

public class ParseTests
{
#region Parse

    [Fact]
    public void SimpleVersion_Parse_Length3_ReturnsCorrectVersion()
    {
        // Arrange
        var expected = new SimpleVersion(new uint[] { 1, 2, 3 });
        string versionString = "1.2.3";

        // Act
        var actual = SimpleVersion.Parse(versionString);

        // Assert
        Assert.True(expected.IsEqualTo(actual));
    }

    [Fact]
    public void SimpleVersion_Parse_Length1WithVUppercase_ReturnsCorrectVersion()
    {
        // Arrange
        var expected = new SimpleVersion(new uint[] { 5 });
        string versionString = "V5";

        // Act
        var actual = SimpleVersion.Parse(versionString);

        // Assert
        Assert.True(expected.IsEqualTo(actual));
    }

    [Fact]
    public void SimpleVersion_Parse_Length8WithVLowerCase_ReturnsCorrectVersion()
    {
        // Arrange
        var expected = new SimpleVersion(new uint[] { 11, 22, 33, 44, 55, 66, 77, 88 });
        string versionString = "v11.22.33.44.55.66.77.88";

        // Act
        var actual = SimpleVersion.Parse(versionString);

        // Assert
        Assert.True(expected.IsEqualTo(actual));
    }

    [Fact]
    public void SimpleVersion_Parse_ZeroLength_Throws()
    {
        // Arrange
        string versionString = String.Empty;

        // Act
        var result = new Func<SimpleVersion>(() => SimpleVersion.Parse(versionString));

        // Assert
        Assert.Throws<ArgumentNullException>(result);
    }

    [Fact]
    public void SimpleVersion_Parse_InvalidString_Throws()
    {
        // Arrange
        string versionString = "Hello World!";

        // Act
        var result = new Func<SimpleVersion>(() => SimpleVersion.Parse(versionString));

        // Assert
        Assert.Throws<FormatException>(result);
    }

    [Fact]
    public void SimpleVersion_Parse_IntOverflow_Throws()
    {
        // Arrange
        string versionString = UInt64.MaxValue.ToString();

        // Act
        var result = new Func<SimpleVersion>(() => SimpleVersion.Parse(versionString));

        // Assert
        Assert.Throws<OverflowException>(result);
    }

#endregion

#region TryParse

    [Fact]
    public void SimpleVersion_TryParse_Length3_ReturnsCorrectVersion()
    {
        // Arrange
        var expected = new SimpleVersion(new uint[] { 1, 2, 3 });
        string versionString = "1.2.3";

        // Act
        bool result = SimpleVersion.TryParse(versionString, out SimpleVersion? actual);

        // Assert
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.True(expected.IsEqualTo(actual!));
    }

    [Fact]
    public void SimpleVersion_TryParse_Length1WithVUppercase_ReturnsCorrectVersion()
    {
        // Arrange
        var expected = new SimpleVersion(new uint[] { 5 });
        string versionString = "V5";

        // Act
        bool result = SimpleVersion.TryParse(versionString, out SimpleVersion? actual);

        // Assert
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.True(expected.IsEqualTo(actual!));
    }

    [Fact]
    public void SimpleVersion_TryParse_Length8WithVLowerCase_ReturnsCorrectVersion()
    {
        // Arrange
        var expected = new SimpleVersion(new uint[] { 11, 22, 33, 44, 55, 66, 77, 88 });
        string versionString = "v11.22.33.44.55.66.77.88";

        // Act
        bool result = SimpleVersion.TryParse(versionString, out SimpleVersion? actual);

        // Assert
        Assert.True(result);
        Assert.NotNull(actual);
        Assert.True(expected.IsEqualTo(actual!));
    }

    [Fact]
    public void SimpleVersion_TryParse_ZeroLength_ReturnsFalse()
    {
        // Arrange
        string versionString = String.Empty;

        // Act
        bool result = SimpleVersion.TryParse(versionString, out SimpleVersion? actual);

        // Assert
        Assert.False(result);
        Assert.Null(actual);
    }

    [Fact]
    public void SimpleVersion_TryParse_InvalidString_ReturnsFalse()
    {
        // Arrange
        string versionString = "Hello World!";

        // Act
        bool result = SimpleVersion.TryParse(versionString, out SimpleVersion? actual);

        // Assert
        Assert.False(result);
        Assert.Null(actual);
    }

    [Fact]
    public void SimpleVersion_TryParse_IntOverflow_ReturnsFalse()
    {
        // Arrange
        string versionString = UInt64.MaxValue.ToString();

        // Act
        bool result = SimpleVersion.TryParse(versionString, out SimpleVersion? actual);

        // Assert
        Assert.False(result);
        Assert.Null(actual);
    }

#endregion
}