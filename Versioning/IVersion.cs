namespace BenjcoreUtil.Versioning;

public interface IVersion
{
    /// <summary>
    /// Checks if the version is newer than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool IsNewerThan(IVersion input);

    /// <summary>
    /// Checks if the version is newer than or equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool IsNewerThanOrEqualTo(IVersion input);

    /// <summary>
    /// Checks if the version is older than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool IsOlderThan(IVersion input);

    /// <summary>
    /// Checks if the version is older than or equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool IsOlderThanOrEqualTo(IVersion input);

    /// <summary>
    /// Checks if the version is equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
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