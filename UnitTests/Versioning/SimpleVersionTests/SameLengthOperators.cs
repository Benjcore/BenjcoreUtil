using System.Diagnostics.CodeAnalysis;
using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

[SuppressMessage("ReSharper", "EqualExpressionComparison")]
public class SameLengthOperators
{
    private static readonly SimpleVersion OlderVersion = new(new uint[] { 1 });
    private static readonly SimpleVersion NewerVersion = new(new uint[] { 2 });

    /*
     * Because these test are very simple, we don't
     * need to follow the arrange/act/assert pattern.
     */

    [Fact]
    public void SimpleVersion_SameLengthOperators_Equals_ReturnsTrueWhenVersionsAreEqual()
    {
        Assert.True(OlderVersion == OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_Equals_ReturnsFalseWhenVersionsAreNotEqual()
    {
        Assert.False(OlderVersion == NewerVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_Equals_ReturnsFalseWhenInputIsNull()
    {
        SimpleVersion? input = null;
        Assert.False(NewerVersion == input);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_Equals_ReturnsFalseWhenCurrentIsNullAndInputIsNotNull()
    {
        SimpleVersion? current = null;
        Assert.False(current == NewerVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_Equals_ReturnsTrueWhenInputAndCurrentIsNull()
    {
        SimpleVersion? current = null;
        SimpleVersion? input = null;
        Assert.True(current == input);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_NotEqual_ReturnsTrueWhenVersionsAreNotEqual()
    {
        Assert.True(OlderVersion != NewerVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_NotEqual_ReturnsFalseWhenVersionsAreEqual()
    {
        Assert.False(OlderVersion != OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_LessThan_ReturnsTrueWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.True(OlderVersion < NewerVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_LessThan_ReturnsFalseWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.False(OlderVersion < OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_LessThan_ReturnsFalseWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.False(NewerVersion < OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_LessThanOrEqual_ReturnsTrueWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.True(OlderVersion <= NewerVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_LessThanOrEqual_ReturnsTrueWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.True(OlderVersion <= OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_LessThanOrEqual_ReturnsFalseWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.False(NewerVersion <= OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_GreaterThan_ReturnsTrueWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.True(NewerVersion > OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_GreaterThan_ReturnsFalseWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.False(OlderVersion > OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_GreaterThan_ReturnsFalseWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.False(OlderVersion > NewerVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_GreaterThanOrEqual_ReturnsTrueWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.True(NewerVersion >= OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_GreaterThanOrEqual_ReturnsTrueWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.True(OlderVersion >= OlderVersion);
    }

    [Fact]
    public void SimpleVersion_SameLengthOperators_GreaterThanOrEqual_ReturnsFalseWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.False(OlderVersion >= NewerVersion);
    }
}