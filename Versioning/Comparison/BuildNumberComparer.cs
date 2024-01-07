namespace BenjcoreUtil.Versioning.Comparison;

/// <summary>
/// A build number based version comparer where the larger build number is considered newer.
/// </summary>
/// <param name="build_number">The current build number.</param>
public sealed class BuildNumberComparer(ulong build_number) : ComparableVersionBase<BuildNumberComparer>
{
    /// <summary>
    /// The build number of the current instance.
    /// </summary>
    public ulong BuildNumber => build_number;
    
    public override (bool NewerThan, bool EqualTo) Compare(BuildNumberComparer other)
    {
        return (BuildNumber > other.BuildNumber, BuildNumber == other.BuildNumber);
    }
}