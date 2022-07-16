namespace BenjcoreUtil.Versioning; 

public interface IVersion
{
    /// <summary>
    /// Checks if the version is newer than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool isNewerThan(IVersion input);
  
    /// <summary>
    /// Checks if the version is newer than or equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool isNewerThanOrEqualTo(IVersion input);
  
    /// <summary>
    /// Checks if the version is older than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool isOlderThan(IVersion input);
  
    /// <summary>
    /// Checks if the version is older than or equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool isOlderThanOrEqualTo(IVersion input);
  
    /// <summary>
    /// Checks if the version is equal to than another version.
    /// </summary>
    /// <param name="input">The version object you wish to compare.</param>
    /// <returns>Boolean Result</returns>
    bool IsEqualTo(IVersion input);

    /// <returns>A string representing the version object.</returns>
    string? ToString();

    #region Operators
    
    public static bool operator >(IVersion x, IVersion y) { return x.isNewerThan(y); }
    public static bool operator <(IVersion x, IVersion y) { return x.isOlderThan(y); }
    public static bool operator >=(IVersion x, IVersion y) { return x.isNewerThanOrEqualTo(y); }
    public static bool operator <=(IVersion x, IVersion y) { return x.isOlderThanOrEqualTo(y); }
    
    #endregion
}