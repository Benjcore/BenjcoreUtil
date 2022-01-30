// ReSharper disable RedundantUsingDirective
// ^ Because of a Rider issue.
using System;
using System.Collections.Generic;
using BenjcoreUtil.Versioning;

namespace UnitTests.Versioning; 

[TestClass]
public class SimpleTests {

  public static readonly Dictionary<Tuple<Simple, Simple>, Tuple<bool, bool, bool, bool, bool>> SampleSimples = new() {
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 0, 1}), new Simple(new []{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, false, true, true, false),
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 0, 0, 1}), new Simple(new []{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, false, true, true, false),
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 1, 0, 1}), new Simple(new []{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(true, true, false, false, false),
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 2, 0, 1}), new Simple(new []{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(true, true, false, false, false),
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 2, 0, 1, 30}), new Simple(new []{1, 166}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, false, true, true, false),
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 1, 0}), new Simple(new []{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, true, false, true, true),
      [Tuple.Create<Simple, Simple>(new Simple(new []{1, 1, 0, 0}), new Simple(new []{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, true, false, true, true)
  };

  [TestMethod]
  public void SimpleVersionTests() {
    
    // Check for NullReferenceException
    try {
      var dummy = new Simple(Array.Empty<int>());
      Assert.IsTrue(false);
    } catch (NullReferenceException) {
      Assert.IsTrue(true);
    }
    
    // Check AllowDifferentLengthComparisons Starts As False
    {
      Simple tst = new Simple(new []{1, 0, 0});
      Assert.IsFalse(tst.AllowDifferentLengthComparisons);
    }
    
    // Check AllowDifferentLengthComparisons Can Be Changed
    {
      Simple tst = new Simple(new []{1, 0, 0});
      tst.AllowDifferentLengthComparisons = true;
      Assert.IsTrue(tst.AllowDifferentLengthComparisons);
    }
    
    // Attempt to compare two Simple Versions of different lengths
    // Without AllowDifferentLengthComparisons:
    try {
      Simple a = new Simple(new []{1, 0, 0});
      Simple b = new Simple(new []{1, 0, 0, 0});
      bool dummy = a.IsEqualTo(b);
      Assert.IsTrue(false);
    } catch (VersioningException) {
      Assert.IsTrue(true);
    }
    // With AllowDifferentLengthComparisons
    {
      Simple a = new Simple(new[] {1, 0, 0});
      Simple b = new Simple(new[] {1, 0, 0, 0});
      a.AllowDifferentLengthComparisons = true;
      bool dummy = a.IsEqualTo(b);
      Assert.IsTrue(true);
    }
    
    // Cover VersioningException
    {
      VersioningException?[] tmp = new[] { new VersioningException(), new VersioningException("Test"), null };
      tmp[2] = new VersioningException("Test 2", tmp[1] ?? throw new InvalidOperationException());
      foreach (var e in tmp) {
        try {
          if (e == null) throw new NullReferenceException();
          throw e;
          #pragma warning disable CS0162
          // ReSharper disable once HeuristicUnreachableCode
          Assert.IsTrue(false);
          #pragma warning restore CS0162
        } catch (VersioningException) {
          Assert.IsTrue(true);
        }
      }
    }


    // Try SampleSimples
    foreach (var item in SampleSimples) {
      
      TestSimple(item, true);

    }
    
  }

  public static void TestSimple(
    KeyValuePair<Tuple<Simple, Simple>, Tuple<bool, bool, bool, bool, bool>> input,
    bool allowDifferentLengthComparisons = false
    ) {
    
    var (key, value) = input;

    key.Item1.AllowDifferentLengthComparisons = allowDifferentLengthComparisons;
    key.Item2.AllowDifferentLengthComparisons = allowDifferentLengthComparisons;
    
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

    foreach (var item in new[]{ key.Item1, key.Item2 }) {
      
      string[] itemStr = item.ToString().Split('.');
      int len = itemStr.Length * 2 - 1;

      object[] tmp = new object[len];
      for (int i = 1; i - 1 < len; i++) {
        
        if (i % 2 == 0) {
          tmp[i - 1] = '.';
        } else {
          // This is Fine because 0 ÷ 2 = 0
          tmp[i - 1] = Int32.Parse(itemStr[(i - 1) / 2]);
        }
        
      }

      for (int i = 1; i - 1 < len; i++) {

        if (i % 2 == 0) {
          Assert.AreEqual('.', (char)tmp[i - 1]);
        } else {
          // This is Fine because 0 ÷ 2 = 0
          Assert.AreEqual(item.Data[(i - 1) / 2], (int)tmp[i - 1]);
        }

      }

    }
    
  }
  
}