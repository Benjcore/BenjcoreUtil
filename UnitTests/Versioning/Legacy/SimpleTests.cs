// ReSharper disable RedundantUsingDirective
// ^ Because of a Rider issue.
#pragma warning disable CS0618
using System;
using System.Collections.Generic;
using BenjcoreUtil.Versioning;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTests.Versioning.Legacy;

/*
 * THIS FILE CONTAINS LEGACY TESTS.
 * THESE TESTS DO NOT NEED TO BE MAINTAINED
 * OR UPDATED TO THE CURRENT CODE STYLE.
 */

[TestClass]
public class SimpleTests {

  public static readonly Dictionary<Tuple<Simple, Simple>, Tuple<bool, bool, bool, bool, bool>> SampleSimples = new() {
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 0, 1}), new Simple(new uint[]{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, false, true, true, false),
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 0, 0, 1}), new Simple(new uint[]{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, false, true, true, false),
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 1, 0, 1}), new Simple(new uint[]{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(true, true, false, false, false),
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 2, 0, 1}), new Simple(new uint[]{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(true, true, false, false, false),
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 2, 0, 1, 30}), new Simple(new uint[]{1, 166}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, false, true, true, false),
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 1, 0}), new Simple(new uint[]{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, true, false, true, true),
      [Tuple.Create<Simple, Simple>(new Simple(new uint[]{1, 1, 0, 0}), new Simple(new uint[]{1, 1, 0}))]=
        Tuple.Create<bool, bool, bool, bool, bool>(false, true, false, true, true)
  };

  [TestMethod]
  public void SimpleVersionTests() {
    
    // Check for NullReferenceException
    try {
      var dummy = new Simple(Array.Empty<uint>());
      Assert.IsTrue(false);
    } catch (ArgumentException) {
      Assert.IsTrue(true);
    }
    
    // Check AllowDifferentLengthComparisons Starts As False
    {
      Simple tst = new Simple(new uint[]{1, 0, 0});
      Assert.IsFalse(tst.AllowDifferentLengthComparisons);
    }
    
    // Check AllowDifferentLengthComparisons Can Be Changed
    {
      Simple tst = new Simple(new uint[]{1, 0, 0});
      tst.AllowDifferentLengthComparisons = true;
      Assert.IsTrue(tst.AllowDifferentLengthComparisons);
    }
    
    // Attempt to compare two Simple Versions of different lengths
    // Without AllowDifferentLengthComparisons:
    try {
      Simple a = new Simple(new uint[]{1, 0, 0});
      Simple b = new Simple(new uint[]{1, 0, 0, 0});
      bool dummy = a.IsEqualTo(b);
      Assert.IsTrue(false);
    } catch (VersioningException) {
      Assert.IsTrue(true);
    }
    // With AllowDifferentLengthComparisons
    {
      Simple a = new Simple(new uint[] {1, 0, 0});
      Simple b = new Simple(new uint[] {1, 0, 0, 0});
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
    
    // Cover Case null == not null
    {
      Simple? tmp = null;
      var dummy = tmp == new Simple(new uint[] {1, 2, 0, 1});
    }
    
    // Equals Tests
    {
      object dummy = new object();
      Simple dummy2 = new Simple(new uint[]{1});
      Simple dummy3 = new Simple(new uint[]{1});
      Simple dummy4 = new Simple(new uint[]{2});
      
      // ReSharper disable once EqualExpressionComparison
      Assert.IsTrue(dummy2.Equals(dummy2));
      Assert.IsTrue(dummy2.Equals(dummy3));
      Assert.IsFalse(dummy2.Equals(dummy));
      Assert.IsFalse(dummy2.Equals(dummy4));
      Assert.AreEqual(dummy.Equals(dummy2), dummy2.Equals(dummy));
    }
    
    // GetHashCode Tests
    {
      Simple dummy = new Simple(new uint[]{1, 0, 89});
      Assert.AreEqual(dummy.Data.GetHashCode(), dummy.GetHashCode());
    }

    // Try SampleSimples
    foreach (var item in SampleSimples) {
      
      TestSimple(item, true);

    }
    
  }

  public static void TestSimple(
    KeyValuePair<Tuple<Simple, Simple>, Tuple<bool, bool, bool, bool, bool>> input,
    bool allowDifferentLengthComparisons = false,
    bool SkipParseTests = false
    ) {
    
    var (key, value) = input;

    key.Item1.AllowDifferentLengthComparisons = allowDifferentLengthComparisons;
    key.Item2.AllowDifferentLengthComparisons = allowDifferentLengthComparisons;
    
    Assert.AreEqual(value.Item1, key.Item1 > key.Item2);
    Assert.AreEqual(value.Item2,key.Item1 >= key.Item2);
    Assert.AreEqual(value.Item3,key.Item1 < key.Item2);
    Assert.AreEqual(value.Item4,key.Item1 <= key.Item2);
    Assert.AreEqual(value.Item5,key.Item1 == key.Item2);
      
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
    
    // ToString() Tests

    foreach (var item in new[]{ key.Item1, key.Item2 }) {
      
      Assert.AreEqual(item.ToString(), (string)item);
      string[] itemStr = item.ToString().Split('.');
      int len = itemStr.Length * 2 - 1;

      object[] tmp = new object[len];
      for (int i = 1; i - 1 < len; i++) {
        
        if (i % 2 == 0) {
          tmp[i - 1] = '.';
        } else {
          // This is Fine because 0 ÷ 2 = 0
          tmp[i - 1] = UInt32.Parse(itemStr[(i - 1) / 2]);
        }
        
      }

      for (int i = 1; i - 1 < len; i++) {

        if (i % 2 == 0) {
          Assert.AreEqual('.', (char)tmp[i - 1]);
        } else {
          // This is Fine because 0 ÷ 2 = 0
          Assert.AreEqual(item.Data[(i - 1) / 2], (uint)tmp[i - 1]);
        }

      }
      
      // Parse & TryParse Tests
      if (!SkipParseTests) {

        // Parse Tests
        {
          
          try {
            var dummy = Simple.Parse(null);
            Assert.IsTrue(false);
          } catch (ArgumentNullException) {
            Assert.IsTrue(true);
          }

          try {
            var dummy = Simple.Parse("uKnxLDzd..UcCADXrPJkQXijASkSX!p6");
            Assert.IsTrue(false);
          } catch (FormatException) {
            Assert.IsTrue(true);
          }
          
          try {
            var dummy = Simple.Parse("uKnxLDzdUcCADXrPJkQXijASkSX!p6");
            Assert.IsTrue(false);
          } catch (FormatException) {
            Assert.IsTrue(true);
          }
          
          try {
            var dummy = Simple.Parse(String.Empty);
            Assert.IsTrue(false);
          } catch (ArgumentNullException) {
            Assert.IsTrue(true);
          }
          
          try {
            var dummy = Simple.Parse($"v{Int64.MaxValue}.{Int64.MaxValue}");
            Assert.IsTrue(false);
          } catch (OverflowException) {
            Assert.IsTrue(true);
          }
          
          var s1 = Simple.Parse(input.Key.Item1.ToString());
          var s2 = Simple.Parse($"V{input.Key.Item2.ToString()}");

          TestSimple(
            new KeyValuePair<Tuple<Simple, Simple>, Tuple<bool, bool, bool, bool, bool>>
              (Tuple.Create<Simple, Simple>(s1, s2), input.Value),
            true,
            true
          );
        }
        
        // TryParse Tests
        {

          {
            Assert.IsFalse(Simple.TryParse(null, out Simple? tst) || tst != null);
            Assert.IsFalse(Simple.TryParse("uKnxLDzd..UcCADXrPJkQXijASkSX!p6", out tst) || tst != null);
            Assert.IsFalse(Simple.TryParse($"v{Int64.MaxValue}.{Int64.MaxValue}", out tst) || tst != null);
          }

          Assert.IsTrue(Simple.TryParse(input.Key.Item1.ToString(), out var s1));
          Assert.IsTrue(Simple.TryParse($"V{input.Key.Item2.ToString()}", out var s2));
          
          Assert.IsTrue(s1 != null);
          Assert.IsTrue(s2 != null);

          TestSimple(
            new KeyValuePair<Tuple<Simple, Simple>, Tuple<bool, bool, bool, bool, bool>>
              (Tuple.Create<Simple, Simple>(s1, s2), input.Value),
            true,
            true
          );
        }

      }
      
      // Other Tests
      Assert.AreEqual(key.Item1 == key.Item2, key.Item1.Equals(key.Item2));

    }
    
  }

}