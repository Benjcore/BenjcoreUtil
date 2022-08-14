using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

public class SameLengthComparison
{
    private static readonly SimpleVersion OlderVersion = new(new uint[] { 1 });
    private static readonly SimpleVersion NewerVersion = new(new uint[] { 2 });

    /*
     * Because these test are very simple, we don't
     * need to follow the arrange/act/assert pattern.
     */

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsEqualTo_ReturnsTrueWhenVersionsAreEqual()
    {
        Assert.True(NewerVersion.IsEqualTo(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsEqualTo_ReturnsFalseWhenVersionsAreNotEqual()
    {
        Assert.False(NewerVersion.IsEqualTo(OlderVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsNewerThan_ReturnTrueWhenCurrentIsNewerThanInput()
    {
        Assert.True(NewerVersion.IsNewerThan(OlderVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsNewerThan_ReturnFalseWhenCurrentIsEqualToInput()
    {
        Assert.False(NewerVersion.IsNewerThan(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsNewerThan_ReturnFalseWhenCurrentIsOlderThanInput()
    {
        Assert.False(OlderVersion.IsNewerThan(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsOlderThan_ReturnTrueWhenCurrentIsOlderThanInput()
    {
        Assert.True(OlderVersion.IsOlderThan(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsOlderThan_ReturnFalseWhenCurrentIsEqualToInput()
    {
        Assert.False(OlderVersion.IsOlderThan(OlderVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsOlderThan_ReturnFalseWhenCurrentIsNewerThanInput()
    {
        Assert.False(NewerVersion.IsOlderThan(OlderVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsNewerThanOrEqualTo_ReturnTrueWhenCurrentIsNewerThanInput()
    {
        Assert.True(NewerVersion.IsNewerThanOrEqualTo(OlderVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsNewerThanOrEqualTo_ReturnTrueWhenCurrentIsEqualToInput()
    {
        Assert.True(NewerVersion.IsNewerThanOrEqualTo(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsNewerThanOrEqualTo_ReturnFalseWhenCurrentIsOlderThanInput()
    {
        Assert.False(OlderVersion.IsNewerThanOrEqualTo(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsOlderThanOrEqualTo_ReturnTrueWhenCurrentIsOlderThanInput()
    {
        Assert.True(OlderVersion.IsOlderThanOrEqualTo(NewerVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsOlderThanOrEqualTo_ReturnTrueWhenCurrentIsEqualToInput()
    {
        Assert.True(OlderVersion.IsOlderThanOrEqualTo(OlderVersion));
    }

    [Fact]
    public void SimpleVersion_SameLengthComparison_IsOlderThanOrEqualTo_ReturnFalseWhenCurrentIsNewerThanInput()
    {
        Assert.False(NewerVersion.IsOlderThanOrEqualTo(OlderVersion));
    }
}