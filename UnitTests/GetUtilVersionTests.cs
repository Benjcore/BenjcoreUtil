global using BenjcoreUtil;
global using System;
global using System.Linq;
global using Xunit;
using System.Collections.Generic;
using System.IO;
using BenjcoreUtil.Versioning;
#pragma warning disable CS0618
using Version = BenjcoreUtil.Versioning.Version;

#pragma warning restore CS0618

namespace UnitTests;

public class GetUtilVersionTests
{
#region Utilities

    private static System.Diagnostics.FileVersionInfo AssemblyVersionAsFileVersionInfo
    {
        get
        {
            // Get the version of the assembly. (BenjcoreUtil.dll)
            System.Reflection.Assembly assembly =
                System.Reflection.Assembly.LoadFile($"{Directory.GetCurrentDirectory()}/BenjcoreUtil.dll");

            return System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        }
    }

    private static uint[] AssemblyVersionAsUIntArray
    {
        get
        {
            // Get the version of the assembly. (BenjcoreUtil.dll)
            var assembly = AssemblyVersionAsFileVersionInfo;

            // Split the version into parts.
            string[] versionParts = assembly.FileVersion!.Split('.');

            // Convert the parts to uints.
            List<uint> list = new();

            foreach (string item in versionParts)
            {
                list.Add(UInt32.Parse(item));
            }

            // Remove last element from list.
            // This is because the last two characters are the build number
            // which is not included in VersionAsString or VersionAsUIntArray.
            list.RemoveAt(list.Count - 1);

            return list.ToArray();
        }
    }

    private static string AssemblyVersionAsString
    {
        get
        {
            // Get the version of the assembly. (BenjcoreUtil.dll)
            uint[] versionAsUIntArray = AssemblyVersionAsUIntArray;

            // Convert to string.
            string version = String.Join(".", versionAsUIntArray);

            // Return the version.
            return version;
        }
    }

#endregion

#region VersionAsUIntArray

    [Fact]
    public void GetUtilVersion_VersionAsUIntArray_LengthIsThree()
    {
        // Arrange
        int expectedLength = 3;

        // Act
        uint[] version = GetUtilVersion.VersionAsUIntArray;

        // Assert
        Assert.Equal(expectedLength, version.Length);
    }

    [Fact]
    public void GetUtilVersion_VersionAsUIntArray_ReturnsCorrectVersion()
    {
        // Arrange
        uint[] expectedVersion = AssemblyVersionAsUIntArray;

        // Act
        uint[] version = GetUtilVersion.VersionAsUIntArray;

        // Assert
        Assert.Equal(expectedVersion[0], version[0]);
        Assert.Equal(expectedVersion[1], version[1]);
        Assert.Equal(expectedVersion[2], version[2]);
    }

#endregion

#region VersionAsString

    [Fact]
    public void GetUtilVersion_VersionAsString_ReturnsVersionAsUIntArrayAsAStringSeperatedByPeriods()
    {
        // Arrange
        uint[] expectedVersion = GetUtilVersion.VersionAsUIntArray;
        string expectedVersionAsString = String.Join(".", expectedVersion);

        // Act
        string version = GetUtilVersion.VersionAsString;

        // Assert
        Assert.Equal(expectedVersionAsString, version);
    }

    [Fact]
    public void GetUtilVersion_VersionAsString_ReturnsCorrectVersion()
    {
        // Arrange
        string expectedVersion = AssemblyVersionAsString;

        // Act
        string version = GetUtilVersion.VersionAsString;

        // Assert
        Assert.Equal(expectedVersion, version);
    }

#endregion

#region VersionAsVersion

#pragma warning disable CS0618

    [Fact]
    public void GetUtilVersion_VersionAsVersion_ReturnsCorrectVersionAsString()
    {
        // Arrange
        string expectedVersion = AssemblyVersionAsString;

        // Act
        Version version = GetUtilVersion.VersionAsVersion;

        // Assert
        Assert.Equal(expectedVersion, version.ToString());
    }

    [Fact]
    public void GetUtilVersion_VersionAsVersion_ReturnsCorrectVersionAsUIntArray()
    {
        // Arrange
        uint[] expectedVersion = AssemblyVersionAsUIntArray;

        // Act
        Version version = GetUtilVersion.VersionAsVersion;
        Simple versionAsSimple = (Simple) version.InnerObject; // Convert to simple

        // Assert
        Assert.Equal(expectedVersion[0], versionAsSimple.Data[0]);
        Assert.Equal(expectedVersion[1], versionAsSimple.Data[1]);
        Assert.Equal(expectedVersion[2], versionAsSimple.Data[2]);
    }

#pragma warning restore CS0618

#endregion

#region VersionAsSimpleVersion

    [Fact]
    public void GetUtilVersion_VersionAsSimpleVersion_ReturnsCorrectVersionAsString()
    {
        // Arrange
        string expectedVersion = AssemblyVersionAsString;

        // Act
        SimpleVersion version = GetUtilVersion.VersionAsSimpleVersion;

        // Assert
        Assert.Equal(expectedVersion, version.ToString());
    }

    [Fact]
    public void GetUtilVersion_VersionAsSimpleVersion_ReturnsCorrectVersionAsUIntArray()
    {
        // Arrange
        uint[] expectedVersion = AssemblyVersionAsUIntArray;

        // Act
        SimpleVersion version = GetUtilVersion.VersionAsSimpleVersion;

        // Assert
        Assert.Equal(expectedVersion[0], version.Data[0]);
        Assert.Equal(expectedVersion[1], version.Data[1]);
        Assert.Equal(expectedVersion[2], version.Data[2]);
    }

#endregion
}