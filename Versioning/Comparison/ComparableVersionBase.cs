namespace BenjcoreUtil.Versioning.Comparison;

/// <summary>
/// A base class for <see cref="IComparableVersion{TSelf}"/> implementations
/// that handles the tedious work of implementing the interface.
/// </summary>
/// <remarks>
/// When referring to a version comparer, use the interface <see cref="IComparableVersion{TSelf}"/>
/// (or <see cref="IComparableVersion"/> if you don't need to specify the type), rather than this class.
/// </remarks>
public abstract class ComparableVersionBase<TSelf> : IComparableVersion<TSelf> where TSelf : IComparableVersion
{
#pragma warning disable CA1859
    private IComparableVersion<TSelf> _version_comparer_implementation => this;
#pragma warning restore CA1859
    
    public abstract (bool NewerThan, bool EqualTo) Compare(TSelf other);
    
    public bool IsNewerThan(IVersion input) => _version_comparer_implementation.IsNewerThan(input);
    public bool IsNewerThanOrEqualTo(IVersion input) => _version_comparer_implementation.IsNewerThanOrEqualTo(input);
    public bool IsOlderThan(IVersion input) => _version_comparer_implementation.IsOlderThan(input);
    public bool IsOlderThanOrEqualTo(IVersion input) => _version_comparer_implementation.IsOlderThanOrEqualTo(input);
    public bool IsEqualTo(IVersion input) => _version_comparer_implementation.IsEqualTo(input);
}