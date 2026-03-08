namespace BenjcoreUtil.Versioning.Comparison;

/// <summary>
/// A variant of <see cref="IVersionComparer"/> that accepts no generic type parameters.
/// To actually use/implement a version comparer, <see cref="IVersionComparer{TSelf}"/>
/// should be used instead. This interface is only intended to be used for loose type checking.
/// </summary>
/// <seealso cref="IVersionComparer{TSelf}"/>
public interface IVersionComparer : IVersion;

/// <summary>
/// An interface that represents an <see cref="IComparableVersion{T}"/> implementation
/// that exists for the sole purpose of being used as a comparer for other
/// <see cref="IVersion"/>/<see cref="IComparableVersion{T}"/> implementations.
/// </summary>
/// <typeparam name="TSelf">The type which implements this interface.</typeparam>
/// <seealso cref="IComparableVersion{TSelf}"/>
/// <seealso cref="BuildNumberComparer"/>
/// <seealso cref="DateTimeComparer"/>
public interface IVersionComparer<in TSelf> : IVersionComparer, IComparableVersion<TSelf> where TSelf : IVersionComparer<TSelf>;