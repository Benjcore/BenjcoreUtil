global using BenjcoreUtil;
global using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests;

[TestClass]
public class GetUtilVersionTests {

  [TestMethod]
  public void VersionTests() {

    int[] test = GetUtilVersion.VersionAsIntArray;
    Assert.AreEqual(3, test.Length);
    Assert.AreEqual($"{test[0]}.{test[2]}.{test[2]}", GetUtilVersion.VersionAsString);
    
  }
  
}