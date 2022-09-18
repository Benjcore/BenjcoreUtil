using System.Diagnostics.CodeAnalysis;
using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

[SuppressMessage("ReSharper", "EqualExpressionComparison")]
public class DifferentLengthOperators
{
    private static readonly SimpleVersion OlderVersionADLCFalse = new(new uint[] { 1, 2 });
    private static readonly SimpleVersion OlderVersionADLCFalse2 = new(new uint[] { 1, 2, 0 });
    private static readonly SimpleVersion NewerVersionADLCFalse = new(new uint[] { 2 });

    private static readonly SimpleVersion OlderVersionADLCTrue = new(new uint[] { 1, 2 })
    {
        AllowDifferentLengthComparisons = true
    };
    
    private static readonly SimpleVersion NewerVersionADLCTrue = new(new uint[] { 2 })
    {
        AllowDifferentLengthComparisons = true
    };

    /*
     * Because these test are very simple, we don't
     * need to follow the arrange/act/assert pattern.
     *
     * ADLCTrue
     * Would mean that SimpleVersion.AllowDifferentLengthComparisons is true.
     *
     * ADLCFalse
     * Would mean that SimpleVersion.AllowDifferentLengthComparisons is false.
     */
    
    
#region ADLCTrue
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_Equals_ADLCTrue_ReturnsTrueWhenVersionsAreEqual()
    {
        Assert.True(OlderVersionADLCTrue == OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_Equals_ADLCTrue_ReturnsFalseWhenVersionsAreNotEqual()
    {
        Assert.False(OlderVersionADLCTrue == NewerVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_NotEqual_ADLCTrue_ReturnsTrueWhenVersionsAreNotEqual()
    {
        Assert.True(OlderVersionADLCTrue != NewerVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_NotEqual_ADLCTrue_ReturnsFalseWhenVersionsAreEqual()
    {
        Assert.False(OlderVersionADLCTrue != OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThan_ADLCTrue_ReturnsTrueWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.True(OlderVersionADLCTrue < NewerVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThan_ADLCTrue_ReturnsFalseWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.False(OlderVersionADLCTrue < OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThan_ADLCTrue_ReturnsFalseWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.False(NewerVersionADLCTrue < OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThanOrEqual_ADLCTrue_ReturnsTrueWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.True(OlderVersionADLCTrue <= NewerVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThanOrEqual_ADLCTrue_ReturnsTrueWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.True(OlderVersionADLCTrue <= OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThanOrEqual_ADLCTrue_ReturnsFalseWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.False(NewerVersionADLCTrue <= OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThan_ADLCTrue_ReturnsTrueWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.True(NewerVersionADLCTrue > OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThan_ADLCTrue_ReturnsFalseWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.False(OlderVersionADLCTrue > OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThan_ADLCTrue_ReturnsFalseWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.False(OlderVersionADLCTrue > NewerVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThanOrEqual_ADLCTrue_ReturnsTrueWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.True(NewerVersionADLCTrue >= OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThanOrEqual_ADLCTrue_ReturnsTrueWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.True(OlderVersionADLCTrue >= OlderVersionADLCTrue);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThanOrEqual_ADLCTrue_ReturnsFalseWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.False(OlderVersionADLCTrue >= NewerVersionADLCTrue);
    }
    
#endregion
    
#region ADLCFalse
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_Equals_ADLCFalse_ThrowsWhenVersionsAreEqual()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse.Equals(NewerVersionADLCFalse));
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_Equals_ADLCFalse_ThrowsWhenVersionsAreNotEqual()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse.Equals(NewerVersionADLCFalse));
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_NotEqual_ADLCFalse_ThrowsWhenVersionsAreNotEqual()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse.Equals(NewerVersionADLCFalse));
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_NotEqual_ADLCFalse_ThrowsWhenVersionsAreEqual()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse.Equals(OlderVersionADLCFalse2));
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThan_ADLCFalse_ThrowsWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse < NewerVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThan_ADLCFalse_ThrowsWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse < OlderVersionADLCFalse2);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThan_ADLCFalse_ThrowsWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => NewerVersionADLCFalse < OlderVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThanOrEqual_ADLCFalse_ThrowsWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse <= NewerVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThanOrEqual_ADLCFalse_ThrowsWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse <= OlderVersionADLCFalse2);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_LessThanOrEqual_ADLCFalse_ThrowsWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => NewerVersionADLCFalse <= OlderVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThan_ADLCFalse_ThrowsWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => NewerVersionADLCFalse > OlderVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThan_ADLCFalse_ThrowsWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse > OlderVersionADLCFalse2);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThan_ADLCFalse_ThrowsWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse > NewerVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThanOrEqual_ADLCFalse_ThrowsWhenLeftVersionIsNewerThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => NewerVersionADLCFalse >= OlderVersionADLCFalse);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThanOrEqual_ADLCFalse_ThrowsWhenLeftVersionIsEqualToRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse >= OlderVersionADLCFalse2);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthOperators_GreaterThanOrEqual_ADLCFalse_ThrowsWhenLeftVersionIsOlderThanRightVersion()
    {
        Assert.Throws<VersioningException>(() => OlderVersionADLCFalse >= NewerVersionADLCFalse);
    }
    
#endregion
}