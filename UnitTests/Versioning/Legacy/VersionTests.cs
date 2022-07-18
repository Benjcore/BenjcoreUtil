// ReSharper disable RedundantUsingDirective
// ^ Because of a Rider issue.
using System;
using System.Collections.Generic;
using BenjcoreUtil.Versioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Version = BenjcoreUtil.Versioning.Version;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests.Legacy.Versioning; 

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
      
      // Cover explicit string conversion.
      Assert.AreEqual(item.Key.Item1.ToString(), (string)item.Key.Item1!);
      Assert.AreEqual(item.Key.Item2.ToString(), (string)item.Key.Item2!);
      Assert.AreEqual((string)(Simple)item.Key.Item1.InnerObject, (string)item.Key.Item1!);
      Assert.AreEqual((string)(Simple)item.Key.Item2.InnerObject, (string)item.Key.Item2!);

    }

  }
  
  public static void TestVersion(
    KeyValuePair<Tuple<Version, Version>, Tuple<bool, bool, bool, bool, bool>> input
  ) {
    
    var (key, value) = input;

    Assert.AreEqual(value.Item1, key.Item1 > key.Item2);
    Assert.AreEqual(value.Item2,key.Item1 >= key.Item2);
    Assert.AreEqual(value.Item3,key.Item1 < key.Item2);
    Assert.AreEqual(value.Item4,key.Item1 <= key.Item2);
    Assert.AreEqual(value.Item5,key.Item1 == key.Item2);
    
    // Cover !=
    Assert.AreEqual(!value.Item5,key.Item1 != key.Item2);
      
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

    // Other Tests
    Assert.AreEqual(key.Item1.InnerObject.ToString(), key.Item1.ToString());
    Assert.AreEqual(key.Item2.InnerObject.ToString(), key.Item2.ToString());
    Assert.AreEqual(key.Item1.InnerObject.GetHashCode(), key.Item1.GetHashCode());
    Assert.AreEqual(key.Item2.InnerObject.GetHashCode(), key.Item2.GetHashCode());
    Assert.AreEqual(key.Item1 == key.Item2, key.Item1.Equals(key.Item2));
    Assert.IsFalse(key.Item1.Equals(new object()));
    Assert.IsFalse(key.Item2.Equals(new object()));

  }
  
}