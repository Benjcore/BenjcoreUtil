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
}