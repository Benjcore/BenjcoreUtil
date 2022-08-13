using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning;

public class IVersionTests
{
    /*
     * This class uses the SimpleVersion class
     * to test the IVersion interface.
     *
     * We do not need to test the logic of the
     * SimpleVersion class, we only need to
     * test that it implements the IVersion
     * interface correctly and that the
     * IVersion interface works as expected.
     *
     * The actual tests for the SimpleVersion
     * class should be in a different file.
     */

    private static readonly IVersion OlderVersion = new SimpleVersion(new uint[] { 1 });
    private static readonly IVersion NewerVersion = new SimpleVersion(new uint[] { 2 });

    /*
     * Because these test are very simple, we don't
     * need to follow the arrange/act/assert pattern.
     */

    [Fact]
    public void SimpleVersion_Implements_IVersion()
    {
        Assert.True(typeof(IVersion).IsAssignableFrom(typeof(SimpleVersion)));
    }

#region Comparisons

    [Fact]
    public void IVersion_IsEqualTo_ReturnsTrueWhenVersionsAreEqual()
    {
        Assert.True(OlderVersion.IsEqualTo(OlderVersion));
        Assert.True(NewerVersion.IsEqualTo(NewerVersion));
    }

    [Fact]
    public void IVersion_IsEqualTo_ReturnsFalseWhenVersionsAreNotEqual()
    {
        Assert.False(OlderVersion.IsEqualTo(NewerVersion));
        Assert.False(NewerVersion.IsEqualTo(OlderVersion));
    }

    [Fact]
    public void IVersion_IsNewerThan_ReturnsTrueWhenCurrentIsNewerThanInput()
    {
        Assert.True(NewerVersion.IsNewerThan(OlderVersion));
    }

    [Fact]
    public void IVersion_IsNewerThan_ReturnsFalseWhenCurrentIsEqualToInput()
    {
        Assert.False(NewerVersion.IsNewerThan(NewerVersion));
    }

    [Fact]
    public void IVersion_IsNewerThan_ReturnsFalseWhenCurrentIsOlderThanInput()
    {
        Assert.False(OlderVersion.IsNewerThan(NewerVersion));
    }

    [Fact]
    public void IVersion_IsOlderThan_ReturnsTrueWhenCurrentIsOlderThanInput()
    {
        Assert.True(OlderVersion.IsOlderThan(NewerVersion));
    }

    [Fact]
    public void IVersion_IsOlderThan_ReturnsFalseWhenCurrentIsEqualToInput()
    {
        Assert.False(OlderVersion.IsOlderThan(OlderVersion));
    }

    [Fact]
    public void IVersion_IsOlderThan_ReturnsFalseWhenCurrentIsNewerThanInput()
    {
        Assert.False(NewerVersion.IsOlderThan(OlderVersion));
    }

    [Fact]
    public void IVersion_IsNewerThanOrEqualTo_ReturnsTrueWhenCurrentIsNewerThanInput()
    {
        Assert.True(NewerVersion.IsNewerThanOrEqualTo(OlderVersion));
    }

    [Fact]
    public void IVersion_IsNewerThanOrEqualTo_ReturnsTrueWhenCurrentIsEqualToInput()
    {
        Assert.True(NewerVersion.IsNewerThanOrEqualTo(NewerVersion));
    }

    [Fact]
    public void IVersion_IsNewerThanOrEqualTo_ReturnsFalseWhenCurrentIsOlderThanInput()
    {
        Assert.False(OlderVersion.IsNewerThanOrEqualTo(NewerVersion));
    }

    [Fact]
    public void IVersion_IsOlderThanOrEqualTo_ReturnsTrueWhenCurrentIsOlderThanInput()
    {
        Assert.True(OlderVersion.IsOlderThanOrEqualTo(NewerVersion));
    }

    [Fact]
    public void IVersion_IsOlderThanOrEqualTo_ReturnsTrueWhenCurrentIsEqualToInput()
    {
        Assert.True(OlderVersion.IsOlderThanOrEqualTo(OlderVersion));
    }

    [Fact]
    public void IVersion_IsOlderThanOrEqualTo_ReturnsFalseWhenCurrentIsNewerThanInput()
    {
        Assert.False(NewerVersion.IsOlderThanOrEqualTo(OlderVersion));
    }

#endregion

#region Operators

    [Fact]
    public void IVersion_GreaterThanOperator_ReturnsTrueWhenLeftIsNewerThanRight()
    {
        Assert.True(NewerVersion > OlderVersion);
    }

    [Fact]
    public void IVersion_GreaterThanOperator_ReturnsFalseWhenLeftIsEqualToRight()
    {
        // ReSharper disable once EqualExpressionComparison
        Assert.False(NewerVersion > NewerVersion);
    }

    [Fact]
    public void IVersion_GreaterThanOperator_ReturnsFalseWhenLeftIsOlderThanRight()
    {
        Assert.False(OlderVersion > NewerVersion);
    }

    [Fact]
    public void IVersion_LessThanOperator_ReturnsTrueWhenLeftIsOlderThanRight()
    {
        Assert.True(OlderVersion < NewerVersion);
    }

    [Fact]
    public void IVersion_LessThanOperator_ReturnsFalseWhenLeftIsEqualToRight()
    {
        // ReSharper disable once EqualExpressionComparison
        Assert.False(OlderVersion < OlderVersion);
    }

    [Fact]
    public void IVersion_LessThanOperator_ReturnsFalseWhenLeftIsNewerThanRight()
    {
        Assert.False(NewerVersion < OlderVersion);
    }

    [Fact]
    public void IVersion_GreaterThanOrEqualToOperator_ReturnsTrueWhenLeftIsNewerThanRight()
    {
        Assert.True(NewerVersion >= OlderVersion);
    }

    [Fact]
    public void IVersion_GreaterThanOrEqualToOperator_ReturnsTrueWhenLeftIsEqualToRight()
    {
        // ReSharper disable once EqualExpressionComparison
        Assert.True(NewerVersion >= NewerVersion);
    }

    [Fact]
    public void IVersion_GreaterThanOrEqualToOperator_ReturnsFalseWhenLeftIsOlderThanRight()
    {
        Assert.False(OlderVersion >= NewerVersion);
    }

    [Fact]
    public void IVersion_LessThanOrEqualToOperator_ReturnsTrueWhenLeftIsOlderThanRight()
    {
        Assert.True(OlderVersion <= NewerVersion);
    }

    [Fact]
    public void IVersion_LessThanOrEqualToOperator_ReturnsTrueWhenLeftIsEqualToRight()
    {
        // ReSharper disable once EqualExpressionComparison
        Assert.True(OlderVersion <= OlderVersion);
    }

    [Fact]
    public void IVersion_LessThanOrEqualToOperator_ReturnsFalseWhenLeftIsNewerThanRight()
    {
        Assert.False(NewerVersion <= OlderVersion);
    }

#endregion

    [Fact]
    public void IVersion_ToString_ReturnsStringRepresentation()
    {
        Assert.Equal("1", OlderVersion.ToString());
        Assert.Equal("2", NewerVersion.ToString());
    }
}