global using System;
global using System.Collections.Generic;
using System.Linq;
using BenjcoreUtil.Versioning;
#pragma warning disable CS0618
using Version = BenjcoreUtil.Versioning.Version;
#pragma warning restore CS0618

namespace BenjcoreUtil;

public static class GetUtilVersion
{
    // Internal version identifiers
    // When bumping the version, don't forget to update BenjcoreUtil.csproj.
    private static uint[] _base_version => [2, 2, 0];
    private static readonly uint? _preview_revision = 1;
    private static readonly string? _preview_branch = "rc"; // Should be the branch's suffix.
    
    /// <summary>
    /// BenjcoreUtil Version as a BenjcoreUtil <see cref="PreviewVersion"/> object.
    /// </summary>
    public static PreviewVersion VersionAsPreviewVersion =>
        new(new SimpleVersion(_base_version), _preview_revision, x => x.SingleOrDefault(b => b.Suffix == _preview_branch));
    
    /// <summary>
    /// BenjcoreUtil Version as a BenjcoreUtil <see cref="SimpleVersion"/> object.
    /// </summary>
    public static SimpleVersion VersionAsSimpleVersion => VersionAsPreviewVersion.SimpleVersion;
    
    /// <summary>
    /// BenjcoreUtil Version as an Int Array.
    /// </summary>
    public static uint[] VersionAsUIntArray => _base_version;
    
    /// <summary>
    /// BenjcoreUtil Version as a String.
    /// </summary>
    /// <remarks>
    /// If the current version is a preview version, only the base version will be returned.
    /// e.g. "1.0.0-rc.1" will return "1.0.0".
    /// </remarks>
    public static string VersionAsString => VersionAsSimpleVersion.ToString();
    
    /// <summary>
    /// BenjcoreUtil Version as a BenjcoreUtil <see cref="Version"/> object.
    /// </summary>
    [Obsolete("GetUtilVersion.VersionAsVersion is deprecated, use GetUtilVersion.VersionAsSimpleVersion instead.")]
#pragma warning disable CS0618
    public static readonly Version VersionAsVersion = new(new Simple(VersionAsUIntArray));
#pragma warning restore CS0618
}