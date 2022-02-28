global using System;
global using System.Collections.Generic;

using BenjcoreUtil.Versioning;
using Version = BenjcoreUtil.Versioning.Version;

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

  /// <summary>
  /// BenjcoreUtil Version as a BenjcoreUtil Version object.
  /// </summary>
  public static readonly Version VersionAsVersion =
    new(new Simple(VersionAsIntArray));

}