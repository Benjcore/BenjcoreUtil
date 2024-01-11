using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning;

public sealed class PreviewVersionTests
{
    [Theory]
    /*          Version1         Version2         GT     EQ   */
    [InlineData("1.0.0",         "1.0.0",         false, true )]
    [InlineData("1.0.0",         "1.0.1",         false, false)]
    [InlineData("1.0.1",         "1.0.0",         true,  false)]
    [InlineData("1.0.0",         "1.1.0.0-pre.1", false, false)]
    [InlineData("1.1.0.0-pre.1", "1.0.0",         true,  false)]
    [InlineData("1.1.0-pre.1",   "1.1.0.0-pre.1", false, true )]
    [InlineData("1.1.0-beta.1",  "1.1.0-alpha.5", true,  false)]
    [InlineData("1.1.0-alpha.5", "1.1.0-beta.1",  false, false)]
    [InlineData("1.1.0-beta.1",  "1.1.0-beta.1",  false, true )]
    [InlineData("1.1.0-beta.1",  "1.1.0-beta.2",  false, false)]
    [InlineData("1.1.0-beta.2",  "1.1.0-beta.1",  true,  false)]
    [InlineData("1.1.0-rc.2",    "1.1.0-rc.2",    false, true )]
    public void PreviewVersion_Comparison_NoComparer(string ver1, string ver2, bool greater_than, bool equal_to)
    {
        // Arrange
        PreviewVersion v1 = PreviewVersion.Parse(ver1);
        PreviewVersion v2 = PreviewVersion.Parse(ver2);
        
        // Act
        var result = v1.Compare(v2);
        var result_newer_than = v1.IsNewerThan(v2);
        var result_newer_than_or_equal_to = v1.IsNewerThanOrEqualTo(v2);
        var result_older_than = v1.IsOlderThan(v2);
        var result_older_than_or_equal_to = v1.IsOlderThanOrEqualTo(v2);
        var result_equal_to = v1.IsEqualTo(v2);
        
        // Assert
        Assert.Equal((greater_than, equal_to), result);
        Assert.Equal(greater_than, result_newer_than);
        Assert.Equal(greater_than || equal_to, result_newer_than_or_equal_to);
        Assert.Equal(!greater_than && !equal_to, result_older_than);
        Assert.Equal(!greater_than || equal_to, result_older_than_or_equal_to);
        Assert.Equal(equal_to, result_equal_to);
        Assert.Equal(greater_than, v1 > v2);
        Assert.Equal(!greater_than && !equal_to, v1 < v2);
        Assert.Equal(greater_than || equal_to, v1 >= v2);
        Assert.Equal(!greater_than || equal_to, v1 <= v2);
        Assert.Equal(equal_to, v1 == v2);
        Assert.Equal(!equal_to, v1 != v2);
    }
}