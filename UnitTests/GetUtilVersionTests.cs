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

    int[] test = GetUtilVersion.VersionAsIntArray;
    Assert.AreEqual(3, test.Length);
    Assert.AreEqual($"{test[0]}.{test[2]}.{test[2]}", GetUtilVersion.VersionAsString);
    Assert.AreEqual(GetUtilVersion.VersionAsString, GetUtilVersion.VersionAsVersion.ToString());
    Assert.AreEqual(new Version(new Simple(GetUtilVersion.VersionAsIntArray)), GetUtilVersion.VersionAsVersion);
    Assert.AreEqual(new Version(Simple.Parse(GetUtilVersion.VersionAsString)), GetUtilVersion.VersionAsVersion);

  }
  
}