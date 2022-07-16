global using BenjcoreUtil;
global using System;
global using Microsoft.VisualStudio.TestTools.UnitTesting;

using BenjcoreUtil.Versioning;
using Version = BenjcoreUtil.Versioning.Version;

namespace UnitTests;

[TestClass]
public class GetUtilVersionTests {

  [TestMethod]
  public void VersionTests() {

    uint[] test = GetUtilVersion.VersionAsUIntArray;
    Assert.AreEqual(3, test.Length);
    Assert.AreEqual($"{test[0]}.{test[1]}.{test[2]}", GetUtilVersion.VersionAsString);
    Assert.AreEqual(GetUtilVersion.VersionAsString, GetUtilVersion.VersionAsVersion.ToString());
    Assert.AreEqual(new Version(new Simple(GetUtilVersion.VersionAsUIntArray)), GetUtilVersion.VersionAsVersion);
    Assert.AreEqual(new Version(Simple.Parse(GetUtilVersion.VersionAsString)), GetUtilVersion.VersionAsVersion);
    Assert.AreEqual(new SimpleVersion(GetUtilVersion.VersionAsUIntArray), GetUtilVersion.VersionAsSimpleVersion);
    Assert.AreEqual(SimpleVersion.Parse(GetUtilVersion.VersionAsString), GetUtilVersion.VersionAsSimpleVersion);

  }
  
}