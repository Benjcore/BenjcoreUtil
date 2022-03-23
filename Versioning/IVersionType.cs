namespace BenjcoreUtil.Versioning; 

/// <summary>
/// Interface for BenjcoreUtil Version Types.
/// </summary>
public interface IVersionType<in Self> {

  /// <summary>
  /// Checks if the version is newer than another version.
  /// </summary>
  /// <param name="input">The version object you wish to compare.</param>
  /// <returns>Boolean Result</returns>
  bool isNewerThan(Self input);
  
  /// <summary>
  /// Checks if the version is newer than or equal to than another version.
  /// </summary>
  /// <param name="input">The version object you wish to compare.</param>
  /// <returns>Boolean Result</returns>
  bool isNewerThanOrEqualTo(Self input);
  
  /// <summary>
  /// Checks if the version is older than another version.
  /// </summary>
  /// <param name="input">The version object you wish to compare.</param>
  /// <returns>Boolean Result</returns>
  bool isOlderThan(Self input);
  
  /// <summary>
  /// Checks if the version is older than or equal to than another version.
  /// </summary>
  /// <param name="input">The version object you wish to compare.</param>
  /// <returns>Boolean Result</returns>
  bool isOlderThanOrEqualTo(Self input);
  
  /// <summary>
  /// Checks if the version is equal to than another version.
  /// </summary>
  /// <param name="input">The version object you wish to compare.</param>
  /// <returns>Boolean Result</returns>
  bool IsEqualTo(Self input);

  /// <returns>A string representing the version object.</returns>
  string? ToString();

}