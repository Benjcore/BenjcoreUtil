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
        bool result_newer_than = v1.IsNewerThan(v2);
        bool result_newer_than_or_equal_to = v1.IsNewerThanOrEqualTo(v2);
        bool result_older_than = v1.IsOlderThan(v2);
        bool result_older_than_or_equal_to = v1.IsOlderThanOrEqualTo(v2);
        bool result_equal_to = v1.IsEqualTo(v2);
        
        if (v2 is PreviewVersion p)
        {
            var result = v1.Compare(p);
            Assert.Equal((greater_than, equal_to), result);
            
            // The strings and hash codes will always be different if one has extra trailing zeros.
            if (v1.SimpleVersion.Length == p.SimpleVersion.Length && !(v1.IsUsingComparer && p.IsUsingComparer))
            {
                bool result_equal_strings = v1.ToString() == p.ToString();
                bool result_equal_hash_code = v1.GetHashCode() == p.GetHashCode();
                
                Assert.Equal(equal_to, result_equal_strings);
                Assert.Equal(equal_to, result_equal_hash_code);
            }
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
        
        if (v2 is PreviewVersion p3)
            Assert.Equal(equal_to, v1.Equals(p3));
        else
            Assert.False(v1.Equals(v2));
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
        Assert.Throws<InvalidOperationException>(result);
    }
    
    [Fact]
    public void PreviewVersion_Comparison_DoesNotUseComparerWhenSimpleVersionsAreDifferent()
    {
        // Arrange
        var comparer_older = new BuildNumberComparer(1000);
        var comparer_newer = new BuildNumberComparer(1001);
        var version_newer = new PreviewVersion(new SimpleVersion([2, 0, 5]), null, _ => null, comparer_older);
        var version_older = new PreviewVersion(new SimpleVersion([1, 5, 9]), null, _ => null, comparer_newer);
        
        // Act & Assert
        AssertComparisons(version_newer, version_older, true, false);
    }
    
    [Fact]
    public void PreviewVersion_Constructor_ThrowsArgumentExceptionWhenBranchIsNullButRevisionIsNot()
    {
        // Act
        Func<PreviewVersion> result = () => new PreviewVersion(new SimpleVersion([1, 0, 0]), 2, _ => null);
        
        // Assert
        Assert.Throws<ArgumentException>(result);
    }
    
    [Fact]
    public void PreviewVersion_Constructor_ThrowsArgumentExceptionWhenBranchIsNotNullButRevisionIs()
    {
        // Act
        Func<PreviewVersion> result = () => new PreviewVersion
            (new SimpleVersion([1, 0, 0]), null, x => x.SingleOrDefault(b => b.Suffix == "alpha"));
        
        // Assert
        Assert.Throws<ArgumentException>(result);
    }
    
    [Fact]
    public void PreviewVersion_Parse_ThrowsArgumentNullExceptionWhenInputIsNullOrEmpty()
    {
        // Act
        var result1 = () => PreviewVersion.Parse(null);
        var result2 = () => PreviewVersion.Parse(String.Empty);
        
        // Assert
        Assert.Throws<ArgumentNullException>(result1);
        Assert.Throws<ArgumentNullException>(result2);
        Assert.False(PreviewVersion.TryParse(null, out _));
        Assert.False(PreviewVersion.TryParse(String.Empty, out _));
    }
    
    [Theory]
    [InlineData("1.1.")]
    [InlineData(".1.1")]
    [InlineData(".")]
    [InlineData("1.0.0test")]
    [InlineData("1.0.0-alpha")]
    [InlineData("1.0.0-alpha1")]
    [InlineData("1.0.0-alpha-1")]
    [InlineData("1.0.0--alpha.1")]
    [InlineData("1.0.0-invalid.4")]
    public void PreviewVersion_Parse_ThrowsFormatExceptionOnInvalidInput(string input)
    {
        // Act
        var result = () => PreviewVersion.Parse(input);
        bool result_try = PreviewVersion.TryParse(input, out var always_null);
        
        // Assert
        Assert.Throws<FormatException>(result);
        Assert.False(result_try);
        Assert.Null(always_null);
    }
}

internal sealed class InvalidComparer : ComparableVersionBase<InvalidComparer>
{
    public override (bool NewerThan, bool EqualTo) Compare(InvalidComparer other) => (false, true);
}