namespace BenjcoreUtil.Versioning.Comparison;

/// <summary>
/// A base class for <see cref="IComparableVersion{TSelf}"/> implementations
/// that handles the tedious work of implementing the interface.
/// </summary>
/// <remarks>
/// When referring to a version comparer, use the interface <see cref="IComparableVersion{TSelf}"/>
/// (or <see cref="IComparableVersion"/> if you don't need to specify the type), rather than this class.
/// </remarks>
public abstract class ComparableVersionBase<TSelf> : IComparableVersion<TSelf> where TSelf : ComparableVersionBase<TSelf>
{
    public abstract (bool NewerThan, bool EqualTo) Compare(TSelf other);
    
    private (bool NewerThan, bool EqualTo) Compare(IVersion other)
    {
        // If other is of type `TSelf`, use the comparer.
        if (other is TSelf comparer)
        {
            return Compare(comparer);
        }
        
        throw new InvalidOperationException($"Cannot compare to version type: '{nameof(other)}'.");
    }
    
    /// <inheritdoc cref="IVersion.IsNewerThan"/>
    /// <throws cref="InvalidOperationException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public bool IsNewerThan(IVersion input) => Compare(input).NewerThan;
    
    /// <inheritdoc cref="IVersion.IsNewerThanOrEqualTo"/>
    /// <throws cref="InvalidOperationException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public bool IsNewerThanOrEqualTo(IVersion input)
    {
        // We assign the result to a variable to avoid calling `Compare` twice.
        var result = Compare(input);
        return result.NewerThan || result.EqualTo;
    }
    
    /// <inheritdoc cref="IVersion.IsOlderThan"/>
    /// <throws cref="InvalidOperationException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public bool IsOlderThan(IVersion input)
    {
        // We assign the result to a variable to avoid calling `Compare` twice.
        var result = Compare(input);
        return result is { NewerThan: false, EqualTo: false };
    }
    
    /// <inheritdoc cref="IVersion.IsOlderThanOrEqualTo"/>
    /// <throws cref="InvalidOperationException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public bool IsOlderThanOrEqualTo(IVersion input)
    {
        // We assign the result to a variable to avoid calling `Compare` twice.
        var result = Compare(input);
        return !result.NewerThan || result.EqualTo;
    }
    
    /// <inheritdoc cref="IVersion.IsEqualTo"/>
    /// <throws cref="InvalidOperationException">
    /// Thrown when <paramref name="input"/> is not of type the correct type.
    /// </throws>
    public bool IsEqualTo(IVersion input) => Compare(input).EqualTo;
}