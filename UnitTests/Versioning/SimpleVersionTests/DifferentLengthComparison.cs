using System.Diagnostics.CodeAnalysis;
using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning.SimpleVersionTests;

// This is because of the custom naming convention.
[SuppressMessage("ReSharper", "CommentTypo")]
[SuppressMessage("ReSharper", "IdentifierTypo")]
public class DifferentLengthComparison
{
    // Note: this test exists because other tests assume AllowDifferentLengthComparisons defaults to false.
    [Fact]
    public void SimpleVersion_AllowDifferentLengthComparisons_DefaultsToFalse()
    {
        // Arrange
        var version = new SimpleVersion(new uint[] { 1 });

        // Act
        bool result = version.AllowDifferentLengthComparisons;

        // Assert
        Assert.False(result);
    }

    /*
     * Method Naming Convention Explanation:
     *
     * ==============================
     *
     * 2To5
     * Would mean that the length of the current version
     * has a length of 2, and it's being compared to a
     * version with a length of 5.
     *
     * VersionWithLengthOf2.ComparisonMethod(VersionWithLengthOf5);
     *
     * ==============================
     *
     * ADLCTrue
     * Would mean that SimpleVersion.AllowDifferentLengthComparisons is true.
     *
     * ADLCFalse
     * Would mean that SimpleVersion.AllowDifferentLengthComparisons is false.
     *
     * ==============================
     *
     * Note: The following can be omitted when the test is expected to throw.
     * 
     * AGTB
     * Would mean that the current version is greater than the version it's being compared to.
     *
     * AEQB
     * Would mean that the current version is equal to the version it's being compared to.
     * 
     * ALTB
     * Would mean that the current version is less than the version it's being compared to.
     *
     * ANEQB
     * Would mean that the current version is not equal to the version it's being compared to.
     * ^ Note: This would only be used in cases where equality
     * is checked rather than if it's greater than or less than.
     */

#region 2To3
    
#region IsEqualTo

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_2To3_ADLCFalse_ANEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        Action comparison = () => current.IsEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_2To3_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });

        // Act
        Action comparison = () => current.IsEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_2To3_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };

        var other = new SimpleVersion(new uint[] { 1, 2, 0 });

        // Act
        bool result = current.IsEqualTo(other);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_2To3_ADLCTrue_ANEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };

        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        bool result = current.IsEqualTo(other);

        // Assert
        Assert.False(result);
    }

#endregion
    
#region IsNewerThan

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_2To3_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        Action comparison = () => current.IsNewerThan(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_2To3_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });

        // Act
        Action comparison = () => current.IsNewerThan(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_2To3_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        Action comparison = () => current.IsNewerThan(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_2To3_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 })
        {
            AllowDifferentLengthComparisons = true
        };

        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        bool result = current.IsNewerThan(other);

        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_2To3_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_2To3_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }

#endregion
    
#region IsNewerThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_2To3_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_2To3_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_2To3_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_2To3_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_2To3_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_2To3_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
#endregion
    
#region IsOlderThan
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_2To3_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });

        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_2To3_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_2To3_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_2To3_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_2To3_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_2To3_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion
    
#region IsOlderThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_2To3_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_2To3_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_2To3_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_2To3_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_2To3_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 0 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_2To3_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion
    
#endregion

#region 9To4
    
#region IsEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_9To4_ADLCFalse_ANEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });

        // Act
        Action comparison = () => current.IsEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_9To4_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_9To4_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_9To4_ADLCTrue_ANEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
#endregion
    
#region IsNewerThan
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_9To4_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsNewerThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_9To4_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsNewerThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_9To4_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        Action comparison = () => current.IsNewerThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_9To4_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_9To4_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_9To4_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }

#endregion

#region IsNewerThanOrEqualTo

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_9To4_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_9To4_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_9To4_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_9To4_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_9To4_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_9To4_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }

#endregion

#region IsOlderThan
    
    [Fact]
public void SimpleVersion_DifferentLengthComparison_IsOlderThan_9To4_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_9To4_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_9To4_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_9To4_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_9To4_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_9To4_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion

#region IsOlderThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_9To4_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_9To4_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_9To4_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_9To4_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_9To4_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_9To4_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2, 3, 4, 5 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion
    
#endregion

#region 7To1

#region IsEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_7To1_ADLCFalse_ANEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_7To1_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_7To1_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_7To1_ADLCTrue_ANEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
#endregion

#region IsNewerThan
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_7To1_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsNewerThan(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_7To1_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsNewerThan(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_7To1_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 2 });

        // Act
        Action comparison = () => current.IsNewerThan(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_7To1_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_7To1_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_7To1_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }

#endregion

#region IsNewerThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_7To1_ADLCFalse_AGTEB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_7To1_ADLCFalse_AEQEB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_7To1_ADLCFalse_ALTEB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 2 });

        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);

        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_7To1_ADLCTrue_AGTEB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_7To1_ADLCTrue_AEQEB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_7To1_ADLCTrue_ALTEB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
#endregion

#region IsOlderThan
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_7To1_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_7To1_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_7To1_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 2 });

        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_7To1_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_7To1_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_7To1_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion

#region IsOlderThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_7To1_ADLCFalse_AGTEB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_7To1_ADLCFalse_AEQEB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 });
        var other = new SimpleVersion(new uint[] { 1 });

        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_7To1_ADLCFalse_ALTEB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 });
        var other = new SimpleVersion(new uint[] { 2 });

        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_7To1_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_7To1_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 0, 0, 0, 0, 0, 0 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_7To1_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 2 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion
    
#endregion

#region 11To14

#region IsEqualTo

    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_11To14_ADLCFalse_ANEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_11To14_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        Action comparison = () => current.IsEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_11To14_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        bool result = current.IsEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsEqualTo_11To14_ADLCTrue_ANEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsEqualTo(other);
        
        // Assert
        Assert.False(result);
    }

#endregion

#region IsNewerThan
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_11To14_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsNewerThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_11To14_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        Action comparison = () => current.IsNewerThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_11To14_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsNewerThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_11To14_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_11To14_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThan_11To14_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsNewerThan(other);
        
        // Assert
        Assert.False(result);
    }
    
#endregion

#region IsNewerThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_11To14_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_11To14_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_11To14_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_11To14_ADLCTrue_AGTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_11To14_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsNewerThanOrEqualTo_11To14_ADLCTrue_ALTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsNewerThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
#endregion

#region IsOlderThan
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_11To14_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_11To14_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_11To14_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsOlderThan(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_11To14_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_11To14_ADLCTrue_AEQB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThan_11To14_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsOlderThan(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion

#region IsOlderThanOrEqualTo
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_11To14_ADLCFalse_AGTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_11To14_ADLCFalse_AEQB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_11To14_ADLCFalse_ALTB_Throws()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        Action comparison = () => current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.Throws<VersioningException>(comparison);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_11To14_ADLCTrue_AGTB_ReturnsFalse()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_11To14_ADLCTrue_AEQB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 0, 0 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void SimpleVersion_DifferentLengthComparison_IsOlderThanOrEqualTo_11To14_ADLCTrue_ALTB_ReturnsTrue()
    {
        // Arrange
        var current = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })
        {
            AllowDifferentLengthComparisons = true
        };
        
        var other = new SimpleVersion(new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
        
        // Act
        bool result = current.IsOlderThanOrEqualTo(other);
        
        // Assert
        Assert.True(result);
    }
    
#endregion
    
#endregion
}