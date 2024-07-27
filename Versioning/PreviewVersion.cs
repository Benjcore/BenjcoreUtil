using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BenjcoreUtil.Versioning.Comparison;

namespace BenjcoreUtil.Versioning;

/// <summary>
/// A version that supports multiple preview branches and revisions. Supported preview branches are:<br/>
/// - Release Candidate (rc)<br/>
/// - Prerelease (pre)<br/>
/// - Beta (beta)<br/>
/// - Alpha (alpha)<br/>
/// </summary>
/// <remarks>
/// If no explicit comparer is set, it is assumed that any version with
/// a more-stable branch is newer than any version with a less-stable branch.
/// e.g. version 1.0.0-rc.1 is newer than 1.0.0-pre.2, which is newer than 1.0.0-beta.6,
/// which is newer than 1.0.0-alpha.12.
/// </remarks>
[SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
public class PreviewVersion : ComparableVersionBase<PreviewVersion>, IParseableVersion<PreviewVersion>
{
    /// <summary>
    /// Creates a new <see cref="PreviewVersion"/> instance.
    /// </summary>
    /// <param name="base_version">
    /// The base <see cref="SimpleVersion"/> for the new preview version.
    /// </param>
    /// <param name="branch_revision">
    /// The revision of the branch for the new preview version, or null if it is not a preview version.
    /// </param>
    /// <param name="branch_selector">
    /// A function that selects the branch for the new preview version,
    /// or either returns null or is null if it is not a preview version.
    /// </param>
    /// <param name="comparer">
    /// An optional <see cref="IComparableVersion"/> to use for comparison.
    /// <see cref="DateTimeComparer"/> and <see cref="BuildNumberComparer"/> are supported.
    /// The comparer will not be used in comparison if the other version does not have the
    /// same comparer set, or their <see cref="SimpleVersion"/>s are not equal.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="branch_revision"/> is not null, but <paramref name="branch_selector"/> returns null,
    /// or vice versa. Also thrown when <paramref name="comparer"/> is not null, but is not of a supported type.
    /// </exception>
    public PreviewVersion(SimpleVersion base_version, uint? branch_revision = null,
        Func<IEnumerable<VersionBranch>, VersionBranch?>? branch_selector = null, IComparableVersion? comparer = null)
    {
        if (comparer is not null)
            SetComparer(comparer); // Will throw an exception if the comparer is not supported.
        
        SimpleVersion = base_version;
        SimpleVersion.AllowDifferentLengthComparisons = true;
        Branch = branch_selector?.Invoke(Branches);
        
        // If the branch is null, we need to ensure that the branch revision is also null.
        if (Branch is null && branch_revision is not null)
            throw new ArgumentException("Branch revision cannot be set if the branch is null.", nameof(branch_revision));
        
        // If the branch is not null, we need to ensure that the branch revision is also not null.
        if (Branch is not null && branch_revision is null)
            throw new ArgumentException("Branch revision must be set if the branch is not null.", nameof(branch_revision));
        
        BranchRevision = branch_revision;
    }
    
    /// <summary>
    /// Sets the <see cref="BuildDate"/> or <see cref="BuildNumber"/> to use for comparison.
    /// </summary>
    /// <param name="comparer">
    /// The <see cref="IComparableVersion"/> to use for comparison.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="comparer"/> is not of a supported type.
    /// </exception>
    public void SetComparer(IComparableVersion comparer)
    {
        switch (comparer)
        {
            case BuildNumberComparer build_number:
                BuildNumber = build_number;
                break;
            
            case DateTimeComparer build_date:
                BuildDate = build_date;
                break;
            
            default:
                throw new ArgumentException("Unsupported type!", nameof(comparer));
        }
    }
    
    /// <summary>
    /// The branches that are available for this version type.
    /// </summary>
    protected virtual ImmutableArray<VersionBranch> Branches =>
    [
        new VersionBranch("Release Candidate", "rc", 1),
        new VersionBranch("Prerelease", "pre", 2),
        new VersionBranch("Beta", "beta", 3),
        new VersionBranch("Alpha", "alpha", 4)
    ];
    
    /// <summary>
    /// The branch of the current preview version.
    /// If this is not a preview version, this will be null.
    /// </summary>
    public VersionBranch? Branch { get; }
    
    /// <summary>
    /// The base <see cref="SimpleVersion"/> of the current preview version.
    /// </summary>
    public SimpleVersion SimpleVersion { get; }
    
    /// <summary>
    /// The revision of the current preview version.
    /// If this is not a preview version, this will be null.
    /// </summary>
    public uint? BranchRevision { get; }
    
    /// <summary>
    /// An optional build date to use for comparison.
    /// </summary>
    public DateTimeComparer? BuildDate { get; private set; } = null;
    
    /// <summary>
    /// An optional build number to use for comparison.
    /// </summary>
    public BuildNumberComparer? BuildNumber { get; private set; } = null;
    
    /// <summary>
    /// Whether or not this version is using a comparer.
    /// </summary>
    public bool IsUsingComparer => BuildNumber is not null || BuildDate is not null;
    
    /// <summary>
    /// Gets the branches that are available for this version.
    /// </summary>
    /// <returns>
    /// An <see cref="ImmutableArray{T}"/> of <see cref="VersionBranch"/>es that are available for this version.
    /// </returns>
    public static ImmutableArray<VersionBranch> GetBranches()
    {
        return new PreviewVersion(new SimpleVersion([0]), null, _ => null).Branches;
    }
    
    public override (bool NewerThan, bool EqualTo) Compare(PreviewVersion other)
    {
        // If both versions are full releases, then we just compare them as SimpleVersions.
        if (Branch is null && other.Branch is null)
            return (SimpleVersion.IsNewerThan(other.SimpleVersion), SimpleVersion.IsEqualTo(other.SimpleVersion));
        
        // If the underlying SimpleVersions are different, we just need to check which one is newer.
        if (!SimpleVersion.IsEqualTo(other.SimpleVersion))
            return (SimpleVersion.IsNewerThan(other.SimpleVersion), false);
        
        // If both versions have build numbers, compare them and return the result.
        if (BuildNumber is not null && other.BuildNumber is not null)
            return BuildNumber.Compare(other.BuildNumber);
        
        // If both versions have build dates, compare them and return the result.
        if (BuildDate is not null && other.BuildDate is not null)
            return BuildDate.Compare(other.BuildDate);
        
        // If the underlying SimpleVersions are the same, but the branches are different,
        // then the version whose branch level is lower, is the newer version.
        // Note: If the branch is null, then it's a full release, aka level 0.
        if ((Branch?.Level ?? 0) != (other.Branch?.Level ?? 0))
            return ((Branch?.Level ?? 0) < (other.Branch?.Level ?? 0), false);
        
        // At this point, we know that both the base versions and the branches are the same,
        // so we now need to return based on what which version has the highest branch revision.
        return
        (
            BranchRevision!.Value > other.BranchRevision!.Value,
            BranchRevision!.Value == other.BranchRevision!.Value
        );
    }
    
    /// <inheritdoc cref="IParseableVersion{TSelf}.Parse"/>
    /// <remarks><paramref name="input"/> may begin with a 'v' (case-insensitive) for styling. The 'v' will be ignored.</remarks>
    public static PreviewVersion Parse([NotNull] string? input)
    {
        if (input is null || input.Length < 1)
            throw new ArgumentNullException(nameof(input));
        
        int dashes = input.Count(c => c is '-');
        
        if (dashes is not 0 and not 1)
            throw new FormatException($"{nameof(input)} was not in a valid format.");
        
        string[] split_version = input.Split('-');
        
        // Will throw the appropriate exceptions if the input is invalid.
        SimpleVersion simple_version = SimpleVersion.Parse(split_version[0]);
        
        // If it is not a preview version, return as-is.
        if (dashes is 0)
            return new PreviewVersion(simple_version, null, _ => null);
        
        if (split_version[1].Count(c => c is '.') is not 1)
            throw new FormatException($"{nameof(input)} was not in a valid format.");
        
        string[] end_split = split_version[1].Split('.');
        string branch_suffix = end_split[0];
        
        // Will throw the appropriate exceptions if the input is invalid.
        uint branch_revision = UInt32.Parse(end_split[1]);
        
        // Checks if the branch suffix is valid.
        if (!GetBranches().Any(b => b.Suffix == branch_suffix))
            throw new FormatException("Branch with given suffix does not exist.");
        
        return new PreviewVersion(simple_version, branch_revision, x => x.First(b => b.Suffix == branch_suffix));
    }
    
    /// <inheritdoc cref="IParseableVersion{TSelf}.TryParse"/>
    /// <remarks><paramref name="input"/> may begin with a 'v' (case-insensitive) for styling. The 'v' will be ignored.</remarks>
    public static bool TryParse([NotNullWhen(true)] string? input, out PreviewVersion? result)
    {
        // Call this default implementation, which will call SimpleVersion.Parse().
        return IParseableVersion<PreviewVersion>.TryParse(input, out result);
    }
    
    public override string ToString()
    {
        if (Branch is null)
        {
            return SimpleVersion.ToString();
        }
        
        return $"{SimpleVersion}-{Branch.Suffix}.{BranchRevision}";
    }
    
    /// <inheritdoc cref="object.Equals(object?)"/>
    /// <remarks>
    /// <b>NOTE: This method does NOT compare references like object.Equals().</b>
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public override bool Equals(object? obj)
    {
        if (obj is IVersion version)
        {
            return IsEqualTo(version);
        }
        
        return false;
    }
    
    /// <inheritdoc cref="object.GetHashCode"/>
    /// <remarks>
    /// <b>NOTE: This method does NOT return the hash code of the reference like object.GetHashCode().</b>
    /// </remarks>
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
    
#region Operators
    [ExcludeFromCodeCoverage]
    public static explicit operator string(PreviewVersion input) => input.ToString();
    
    [ExcludeFromCodeCoverage]
    public static bool operator ==(PreviewVersion? x, PreviewVersion? y)
    {
        if ((object?) y is null)
            return (object?) x is null;
        
        if ((object?) x is null)
            return false;
        
        return x.IsEqualTo(y);
    }
    
    [ExcludeFromCodeCoverage] public static bool operator !=(PreviewVersion? x, PreviewVersion? y) => !(x == y);
    [ExcludeFromCodeCoverage] public static bool operator >(PreviewVersion x, PreviewVersion y) => x.IsNewerThan(y);
    [ExcludeFromCodeCoverage] public static bool operator <(PreviewVersion x, PreviewVersion y) => x.IsOlderThan(y);
    [ExcludeFromCodeCoverage] public static bool operator >=(PreviewVersion x, PreviewVersion y) => x.IsNewerThanOrEqualTo(y);
    [ExcludeFromCodeCoverage] public static bool operator <=(PreviewVersion x, PreviewVersion y) => x.IsOlderThanOrEqualTo(y);
#endregion
}