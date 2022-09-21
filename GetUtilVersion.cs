global using System;
global using System.Collections.Generic;
using BenjcoreUtil.Versioning;
#pragma warning disable CS0618
using Version = BenjcoreUtil.Versioning.Version;

#pragma warning restore CS0618

namespace BenjcoreUtil;

public static class GetUtilVersion
{
    /// <summary>
    /// BenjcoreUtil Version as an Int Array.
    /// </summary>
    public static readonly uint[] VersionAsUIntArray = { 2, 1, 1 };

    /// <summary>
    /// BenjcoreUtil Version as a String.
    /// </summary>
    public static readonly string VersionAsString =
        $"{VersionAsUIntArray[0]}.{VersionAsUIntArray[1]}.{VersionAsUIntArray[2]}";

    /// <summary>
    /// BenjcoreUtil Version as a BenjcoreUtil <see cref="Version"/> object.
    /// </summary>
    [Obsolete("GetUtilVersion.VersionAsVersion is deprecated, use GetUtilVersion.VersionAsSimpleVersion instead.")]
#pragma warning disable CS0618
    public static readonly Version VersionAsVersion = new(new Simple(VersionAsUIntArray));
#pragma warning restore CS0618

    /// <summary>
    /// BenjcoreUtil Version as a BenjcoreUtil <see cref="SimpleVersion"/> object.
    /// </summary>
    public static readonly SimpleVersion VersionAsSimpleVersion = new(VersionAsUIntArray);
}