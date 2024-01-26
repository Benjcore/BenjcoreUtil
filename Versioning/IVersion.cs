namespace BenjcoreUtil.Versioning;

public interface IVersion
{
    /// <summary>
    /// Checks if the version is newer than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare to the current.</param>
    /// <returns>
    /// A boolean indicating whether or not the current version is newer than the given version.
    /// </returns>
    bool IsNewerThan(IVersion input);
    
    /// <summary>
    /// Checks if the version is newer than or equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare to the current.</param>
    /// <returns>
    /// A boolean indicating whether or not the current version is newer than or equal to the given version.
    /// </returns>
    bool IsNewerThanOrEqualTo(IVersion input);
    
    /// <summary>
    /// Checks if the version is older than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare to the current.</param>
    /// <returns>
    /// A boolean indicating whether or not the current version is older than the given version.
    /// </returns>
    bool IsOlderThan(IVersion input);
    
    /// <summary>
    /// Checks if the version is older than or equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare to the current.</param>
    /// <returns>
    /// A boolean indicating whether or not the current version is older than or equal to the given version.
    /// </returns>
    bool IsOlderThanOrEqualTo(IVersion input);
    
    /// <summary>
    /// Checks if the version is equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare to the current.</param>
    /// <returns>
    /// A boolean indicating whether or not the current version is equal to the given version.
    /// </returns>
    bool IsEqualTo(IVersion input);
    
    /// <returns>A string representing the version object.</returns>
    string? ToString();
    
#region Operators
    public static bool operator >(IVersion x, IVersion y) { return x.IsNewerThan(y); }
    public static bool operator <(IVersion x, IVersion y) { return x.IsOlderThan(y); }
    public static bool operator >=(IVersion x, IVersion y) { return x.IsNewerThanOrEqualTo(y); }
    public static bool operator <=(IVersion x, IVersion y) { return x.IsOlderThanOrEqualTo(y); }
#endregion
}
