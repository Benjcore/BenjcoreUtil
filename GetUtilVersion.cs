// ReSharper disable UnusedMember.Global

namespace BenjcoreUtil;

public static class GetUtilVersion {
  
  /// <summary>
  /// BenjcoreUtil Version as an Int Array.
  /// </summary>
  public static readonly int[] VersionAsIntArray = { 2, 0, 0 };
  
  /// <summary>
  /// BenjcoreUtil Version as a String.
  /// </summary>
  public static readonly string VersionAsString = 
    $"{VersionAsIntArray[0]}.{VersionAsIntArray[1]}.{VersionAsIntArray[2]}";
  
  // TODO: Add Simple3 Version

}