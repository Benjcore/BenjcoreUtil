using System.Linq;

namespace BenjcoreUtil.Versioning;

/// <summary>
/// Represents a version branch.
/// </summary>
/// <param name="Name">
/// A unique name for the branch.
/// </param>
/// <param name="Suffix">
/// A suffix to append to the version when formatted as a string, must be comprised of only letters.
/// For example, if the suffix is "beta", the version will be formatted something like: "1.0.0-beta.1".
/// </param>
/// <param name="Level">
/// The level of the branch. Higher levels are considered newer or more unstable.
/// DO NOT use Level 0. This is reserved for the main release branch.
/// </param>
/// <remarks>
/// <paramref name="Name"/> and <paramref name="Suffix"/> cannot be null or empty.
/// </remarks>
public record VersionBranch(string Name, string Suffix, int Level);

public static class VersionBranchExtensions
{
    /// <summary>
    /// Validates the <see cref="VersionBranch"/> to ensure it is valid.
    /// </summary>
    /// <param name="branch">This <see cref="VersionBranch"/> instance.</param>
    /// <exception cref="FormatException">
    /// <paramref name="branch"/> is invalid.
    /// </exception>
    public static void Validate(this VersionBranch branch)
    {
        if (branch.Level is 0)
        {
            throw new FormatException("Level 0 is reserved for the main release branch.");
        }
        
        if (String.IsNullOrEmpty(branch.Name) || String.IsNullOrEmpty(branch.Suffix))
        {
            throw new FormatException("Branch name and or suffix cannot be null or empty.");
        }
        
        if (branch.Suffix.Any(c => !Char.IsLetter(c)))
        {
            throw new FormatException("Branch suffix must be comprised of only letters.");
        }
    }
}