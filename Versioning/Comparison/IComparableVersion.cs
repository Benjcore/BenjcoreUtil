namespace BenjcoreUtil.Versioning.Comparison;

/// <summary>
/// A variant of <see cref="IComparableVersion{TSelf}"/> with no type parameter.
/// </summary>
public interface IComparableVersion : IVersion;

/// <summary>
/// A version comparer that supports comparison to other versions of the same type.
/// </summary>
/// <typeparam name="TSelf">
/// The type of the class that implements this interface.
/// </typeparam>
public interface IComparableVersion<in TSelf> : IComparableVersion where TSelf : IComparableVersion
{
    /// <summary>
    /// Compares the current <see cref="IComparableVersion{TSelf}"/> instance to another.
    /// </summary>
    /// <param name="other">The other <see cref="IComparableVersion{TSelf}"/> instance to compare to.</param>
    /// <returns>
    /// A tuple where the first bool indicates whether or not the current instance is newer than the other,
    /// and the second bool indicates whether or not the current instance is equal to the other.
    /// </returns>
    /// <throws cref="ArgumentException">
    /// Thrown when <paramref name="other"/> is not of type the correct type.
    /// </throws>
    protected (bool NewerThan, bool EqualTo) Compare(TSelf other);
    
    private (bool NewerThan, bool EqualTo) Compare(IVersion other)
    {
        // If other is of type `TSelf`, use the comparer.
        if (other is TSelf comparer)
        {
            return Compare(comparer);
        }
        
        throw new ArgumentException("Cannot compare to the given version type.", nameof(other));
    }
    
    /// <inheritdoc cref="IVersion.IsNewerThan"/>
    /// <throws cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public new bool IsNewerThan(IVersion input) => Compare(input).NewerThan;
    
    /// <inheritdoc cref="IVersion.IsNewerThanOrEqualTo"/>
    /// <throws cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public new bool IsNewerThanOrEqualTo(IVersion input)
    {
        // We assign the result to a variable to avoid calling `Compare` twice.
        var result = Compare(input);
        return result.NewerThan || result.EqualTo;
    }
    
    /// <inheritdoc cref="IVersion.IsOlderThan"/>
    /// <throws cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public new bool IsOlderThan(IVersion input) => !Compare(input).NewerThan;
    
    /// <inheritdoc cref="IVersion.IsOlderThanOrEqualTo"/>
    /// <throws cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public new bool IsOlderThanOrEqualTo(IVersion input)
    {
        // We assign the result to a variable to avoid calling `Compare` twice.
        var result = Compare(input);
        return !result.NewerThan || result.EqualTo;
    }
    
    /// <inheritdoc cref="IVersion.IsEqualTo"/>
    /// <throws cref="ArgumentException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public new bool IsEqualTo(IVersion input) => Compare(input).EqualTo;
}