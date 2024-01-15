using System.Collections.Generic;
using BenjcoreUtil.Versioning;
using BenjcoreUtil.Versioning.Comparison;

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
        
        // Assert & Act
        AssertComparisons(v1, v2, greater_than, equal_to);
    }
    
    private static void AssertComparisons(PreviewVersion v1, IVersion v2, bool greater_than, bool equal_to)
    {
        var result_newer_than = v1.IsNewerThan(v2);
        var result_newer_than_or_equal_to = v1.IsNewerThanOrEqualTo(v2);
        var result_older_than = v1.IsOlderThan(v2);
        var result_older_than_or_equal_to = v1.IsOlderThanOrEqualTo(v2);
        var result_equal_to = v1.IsEqualTo(v2);
        
        if (v2 is PreviewVersion p)
        {
            var result = v1.Compare(p);
            Assert.Equal((greater_than, equal_to), result);
        }
        
        Assert.Equal(greater_than, result_newer_than);
        Assert.Equal(greater_than || equal_to, result_newer_than_or_equal_to);
        Assert.Equal(!greater_than && !equal_to, result_older_than);
        Assert.Equal(!greater_than || equal_to, result_older_than_or_equal_to);
        Assert.Equal(equal_to, result_equal_to);
        Assert.Equal(greater_than, v1 > v2);
        Assert.Equal(!greater_than && !equal_to, v1 < v2);
        Assert.Equal(greater_than || equal_to, v1 >= v2);
        Assert.Equal(!greater_than || equal_to, v1 <= v2);
        
        if (v2 is PreviewVersion p2)
        {
            Assert.Equal(equal_to, v1 == p2);
            Assert.Equal(!equal_to, v1 != p2);
        }
    }
    
    [Theory]
    /*          BuildNumber1     BuildNumber2     GT     EQ   */
    [InlineData(1,               1,               false, true )]
    [InlineData(1,               2,               false, false)]
    [InlineData(2,               1,               true,  false)]
    [InlineData(1,               UInt64.MaxValue, false, false)]
    [InlineData(UInt64.MaxValue, 1, true,  false)]
    public void PreviewVersion_Comparison_BuildNumber(ulong build_number1, ulong build_number2, bool greater_than, bool equal_to)
    {
        // Arrange
        var rng = new Random();
        var alpha = (IEnumerable<VersionBranch> x) => x.SingleOrDefault(b => b.Suffix == "alpha");
        PreviewVersion v1 = new(new SimpleVersion([1, 0, 0]), (uint)rng.Next(), alpha, new BuildNumberComparer(build_number1));
        PreviewVersion v2 = new(new SimpleVersion([1, 0, 0]), (uint)rng.Next(), alpha, new BuildNumberComparer(build_number2));
        
        // Assert & Act
        AssertComparisons(v1, v2, greater_than, equal_to);
    }
    
    [Theory]
    /*          Timestamp1      Timestamp2      GT     EQ   */
    [InlineData(1632432000L,    1632432000L,    false, true )]
    [InlineData(1632432000L,    1632432001L,    false, false)]
    [InlineData(1632432001L,    1632432000L,    true,  false)]
    [InlineData(1632432000L,    1632431999L,    true,  false)]
    [InlineData(Int32.MinValue, Int32.MaxValue, false, false)]
    [InlineData(Int32.MaxValue, Int32.MinValue, true,  false)]
    public void PreviewVersion_Comparison_DateTime(long timestamp1, long timestamp2, bool greater_than, bool equal_to)
    {
        // Arrange
        var rng = new Random();
        var alpha = (IEnumerable<VersionBranch> x) => x.SingleOrDefault(b => b.Suffix == "alpha");
        var dt1 = new DateTimeComparer(DateTimeOffset.FromUnixTimeSeconds(timestamp1).UtcDateTime);
        var dt2 = new DateTimeComparer(DateTimeOffset.FromUnixTimeSeconds(timestamp2).UtcDateTime);
        PreviewVersion v1 = new(new SimpleVersion([1, 0, 0]), (uint)rng.Next(), alpha, dt1);
        PreviewVersion v2 = new(new SimpleVersion([1, 0, 0]), (uint)rng.Next(), alpha, dt2);
        
        // Assert & Act
        AssertComparisons(v1, v2, greater_than, equal_to);
    }
    
    [Fact]
    public void PreviewVersion_SetComparer_ThrowsArgumentExceptionOnInvalidComparer()
    {
        // Arrange
        PreviewVersion version = new(new SimpleVersion([1, 0, 0]), null, _ => null);
        
        // Act
        var result = () => version.SetComparer(new InvalidComparer());
        
        // Assert
        Assert.Throws<ArgumentException>(result);
    }
    
    [Fact]
    public void PreviewVersion_Comparison_ThrowsArgumentExceptionOnInvalidType()
    {
        // Arrange
        PreviewVersion version = new(new SimpleVersion([1, 0, 0]), null, _ => null);
        
        // Act
        var result = () => AssertComparisons(version, version.SimpleVersion, false, true);
        
        // Assert
        Assert.Throws<ArgumentException>(result);
    }
}

internal sealed class InvalidComparer : ComparableVersionBase<InvalidComparer>
{
    public override (bool NewerThan, bool EqualTo) Compare(InvalidComparer other) => (false, true);
}