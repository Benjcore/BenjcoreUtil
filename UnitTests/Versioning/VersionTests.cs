// ReSharper disable RedundantUsingDirective
// ^ Because of a Rider issue.
using System;
using System.Collections.Generic;
using BenjcoreUtil.Versioning;
using Version = BenjcoreUtil.Versioning.Version;

namespace UnitTests.Versioning; 

[TestClass]
public class VersionTests {

  public static readonly Dictionary<Tuple<Version, Version>, Tuple<bool, bool, bool, bool, bool>> SampleVersions;
  
  // Static Constructor
  static VersionTests() {
    
    // Reference SampleSimples From SimpleTests.cs
    var SampleSimples = SimpleTests.SampleSimples;

    // Create Temporary Dictionary
    Dictionary<Tuple<Version, Version>, Tuple<bool, bool, bool, bool, bool>> tempDictionary = new();

    // Convert Simples to Versions
    foreach (var item in SampleSimples) {

      var tmp = Tuple.Create<Version, Version>(
        new Version(new Simple(item.Key.Item1.Data) {AllowDifferentLengthComparisons = true}),
        new Version(new Simple(item.Key.Item2.Data) {AllowDifferentLengthComparisons = true})
      );
      
      tempDictionary.Add(tmp, item.Value);
      
    }
    
    // Copy Temporary Dictionary Onto Real Dictionary
    SampleVersions = new Dictionary<Tuple<Version, Version>, Tuple<bool, bool, bool, bool, bool>>(tempDictionary);

  }

  [TestMethod]
  public void VersionObjectTests() {
    
    // Cover VersioningException
    {
      try {

        var dummy = new Version('F');
        Assert.IsTrue(false);
        
      } catch (VersioningException) {
        Assert.IsTrue(true);
      }
    }

    foreach (var item in SampleVersions) {

      TestVersion(item);

    }

  }
  
  public static void TestVersion(
    KeyValuePair<Tuple<Version, Version>, Tuple<bool, bool, bool, bool, bool>> input
  ) {
    
    var (key, value) = input;

    Assert.AreEqual(value.Item1, key.Item1.isNewerThan(key.Item2));
    Assert.AreEqual(value.Item2,key.Item1.isNewerThanOrEqualTo(key.Item2));
    Assert.AreEqual(value.Item3,key.Item1.isOlderThan(key.Item2));
    Assert.AreEqual(value.Item4,key.Item1.isOlderThanOrEqualTo(key.Item2));
    Assert.AreEqual(value.Item5,key.Item1.IsEqualTo(key.Item2));
      
    // Backwards Tests
    if (value.Item5 /* Equal */) {
      Assert.AreEqual(value.Item1, key.Item2.isNewerThan(key.Item1));
      Assert.AreEqual(value.Item2, key.Item2.isNewerThanOrEqualTo(key.Item1));
      Assert.AreEqual(value.Item3, key.Item2.isOlderThan(key.Item1));
      Assert.AreEqual(value.Item4, key.Item2.isOlderThanOrEqualTo(key.Item1));
    } else {
      Assert.AreEqual(!value.Item1, key.Item2.isNewerThan(key.Item1));
      Assert.AreEqual(!value.Item2, key.Item2.isNewerThanOrEqualTo(key.Item1));
      Assert.AreEqual(!value.Item3, key.Item2.isOlderThan(key.Item1));
      Assert.AreEqual(!value.Item4, key.Item2.isOlderThanOrEqualTo(key.Item1));
    }

    {
      Assert.AreEqual(value.Item5, key.Item2.IsEqualTo(key.Item1));
    }

    Assert.AreEqual(key.Item1.ToString(), key.Item1.InnerObject.ToString());
    Assert.AreEqual(key.Item2.ToString(), key.Item2.InnerObject.ToString());
    
  }
  
}