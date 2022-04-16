using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenjcoreUtil.Logging;

namespace UnitTests.Logging; 

[TestClass]
public class LoggerTests {
  
  internal const string LogFile = "test.log";
  internal const string SampleMessage = "This is a test message.";
  internal static readonly Action<string> SampleAction = Console.WriteLine;
  
  internal static readonly TextWriter StdOut = Console.Out;
  private static StringWriter StringWriter = new();
  private static LogStyle Style = LogStyle.SampleLogStyleYMD;
  
  public static LogLevel[] SampleLogLevels = {
    new("FATAL", 0, LogSettings.PrintAndLog),
    new("ERROR", 1, LogSettings.JustLog),
    new("WARN", 2, LogSettings.JustLog),
    new("PRINT", 3, LogSettings.JustPrint),
    new("INFO", 4, LogSettings.JustLog),
    new("DEBUG", 5, LogSettings.Nothing)
  };

  [TestMethod]
  public void LoggerTest() {

    // Redirect Console Output
    Console.SetOut(StringWriter);

    ////////////////////////////////

    // LogStyle Exception Tests
    
    try {
      #pragma warning disable CS0219
      LogStyle dummy = "%t}";
      #pragma warning restore CS0219
      Assert.IsTrue(false);
    } catch (FormatException) {
      Assert.IsTrue(true);
    }
    
    try {
      #pragma warning disable CS0219
      LogStyle dummy = "%t";
      #pragma warning restore CS0219
      Assert.IsTrue(false);
    } catch (FormatException) {
      Assert.IsTrue(true);
    }
    
    try {
      #pragma warning disable CS0219
      LogStyle dummy = "%";
      #pragma warning restore CS0219
      Assert.IsTrue(false);
    } catch (FormatException) {
      Assert.IsTrue(true);
    }
    
    try {
      #pragma warning disable CS0219
      LogStyle dummy = "%p";
      #pragma warning restore CS0219
      Assert.IsTrue(false);
    } catch (FormatException) {
      Assert.IsTrue(true);
    }
    
    // LogLevel Operation Tests
    {
      var dict = new List<Tuple<LogLevel, LogLevel>>();
      
      foreach (var i in SampleLogLevels) {
        foreach (var j in SampleLogLevels) {
          dict.Add(Tuple.Create<LogLevel, LogLevel>(i, j));
        }
      }

      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Severity == i.Item2.Severity, i.Item1 == i.Item2);
      }
      
      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Severity != i.Item2.Severity, i.Item1 != i.Item2);
      }
      
      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Severity >= i.Item2.Severity, i.Item1 >= i.Item2);
      }
      
      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Severity <= i.Item2.Severity, i.Item1 <= i.Item2);
      }
      
      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Severity > i.Item2.Severity, i.Item1 > i.Item2);
      }
      
      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Severity < i.Item2.Severity, i.Item1 < i.Item2);
      }
      
      // LogLevel Hash Tests
      foreach (var i in dict) {
        Assert.AreEqual(i.Item1.Equals(i.Item2), i.Item1.GetHashCode() == i.Item2.GetHashCode());
      }
      
      foreach (var i in dict) {
        LogLevel tmp = new(i.Item2.Name, Int32.MaxValue, LogSettings.Nothing);
        Assert.AreEqual(i.Item1.Equals(tmp), i.Item1.GetHashCode() == tmp.GetHashCode());
      }
    }
    
    // Other LogLevel Tests
    
    foreach (var i in SampleLogLevels) {
      
      Assert.IsFalse(SampleLogLevels[0].Equals(new object()));
      Assert.AreEqual(i.ToString(), i.Name);
      
    }
    
    try {
      var dummy = new LogLevel("Test", 0, (LogSettings)Int16.MaxValue);
      Assert.IsTrue(false);
    } catch (InvalidEnumArgumentException) {
      Assert.IsTrue(true);
    }
    
    // Main Logger Tests
    
    {
      var tmp = TestLogger(new Logger(SampleLogLevels, Style, LogFile, singleStream: false));
      while (!tmp.IsCompleted) {}
      if (tmp.IsFaulted && tmp.Exception is not null) throw tmp.Exception;
    }

    {
      var tmp = TestLogger(new Logger(SampleLogLevels, "[%n] %% [%l] %t{yy/MM/dd HH:mm:ss} : ",
        LogFile, "Single Stream Logger", true));
      while (!tmp.IsCompleted) { }
      if (tmp.IsFaulted && tmp.Exception is not null) throw tmp.Exception;
    }
    
    WipeStream();
    
    // LogLevel Function Tests
    {
      LogLevel[] levels = SampleLogLevels;
      levels[1] = new LogLevel("ERROR", 1, LogSettings.JustLog, SampleAction, false);
      Logger logger = new Logger(levels, Style, LogFile);

      logger.Log(levels[1], SampleMessage);
      StringReader reader = new StringReader(StringWriter.ToString());
      string res = reader.ReadToEnd();
      res = res.Replace("\n\r\n", "\n");
      Assert.AreEqual(Style.FormatEvent(levels[1], logger.Name, SampleMessage), res);
      WipeStream();
      logger.Dispose();
    }
    
    {
      LogLevel[] levels = SampleLogLevels;
      levels[1] = new LogLevel("ERROR", 1, LogSettings.JustLog, SampleAction, true);
      Logger logger = new Logger(levels, Style, LogFile);
      
      logger.Log(levels[1], SampleMessage);
      StringReader reader = new StringReader(StringWriter.ToString());
      string res = reader.ReadToEnd();
      res = res.Replace("\r\n", "");
      Assert.AreEqual(SampleMessage, res);
      WipeStream();
      logger.Dispose();
    }
    
    // Logger Exceptions
    //
    // No LogLevels
    try {
      var dummy = new Logger(Array.Empty<LogLevel>(), Style, LogFile);
      Assert.IsTrue(false);
    } catch (ArgumentException) {
      Assert.IsTrue(true);
    }
    
    // Duplicate LogLevel Severities
    try {
      var dummy = new Logger(new LogLevel[] {
        new("Test1", 0, LogSettings.Nothing),
        new("Test2", 0, LogSettings.Nothing)},
        Style, LogFile
        );
      Assert.IsTrue(false);
    } catch (ArgumentException) {
      Assert.IsTrue(true);
    }
    
    // Same LogLevels
    try {
      LogLevel tmp = new("Test1", 0, LogSettings.Nothing);
      var dummy = new Logger(new LogLevel[] {tmp, tmp}, Style, LogFile);
      Assert.IsTrue(false);
    } catch (ArgumentException) {
      Assert.IsTrue(true);
    }
    
    // Duplicate LogLevel Names
    try {
      var dummy = new Logger(new LogLevel[] {
          new("Test", 0, LogSettings.Nothing),
          new("Test", 1, LogSettings.Nothing)},
        Style, LogFile
      );
      Assert.IsTrue(false);
    } catch (ArgumentException) {
      Assert.IsTrue(true);
    }
    
    // Call Logger.Log() with invalid LogLevel.
    {
      Logger logger = new(SampleLogLevels, Style, LogFile);
      LogLevel testLevel1 = new("Invalid Level Test", Int16.MaxValue, LogSettings.PrintAndLog);
      LogLevel testLevel2 = new("Invalid Level Test", 1, LogSettings.PrintAndLog);
      
      // Test with unique severity.
      try {
        logger.Log(testLevel1, SampleMessage);
        Assert.IsTrue(false);
      } catch (ArgumentException) {
        Assert.IsTrue(true);
      }
      
      // Test with overlapping severity.
      try {
        logger.Log(testLevel2, SampleMessage);
        Assert.IsTrue(false);
      } catch (ArgumentException) {
        Assert.IsTrue(true);
      }
    }
    
    // Call Logger.Log() with invalid string.
    {
      Logger logger = new(SampleLogLevels, Style, LogFile);
      LogLevel testLevel1 = new("Invalid Level Test", Int16.MaxValue, LogSettings.PrintAndLog);
      LogLevel testLevel2 = new("Invalid Level Test", 1, LogSettings.PrintAndLog);
      
      // Test with unique severity.
      try {
        logger.Log(testLevel1.Name, SampleMessage);
        Assert.IsTrue(false);
      } catch (ArgumentException) {
        Assert.IsTrue(true);
      }
      
      // Test with overlapping severity.
      try {
        logger.Log(testLevel2.Name, SampleMessage);
        Assert.IsTrue(false);
      } catch (ArgumentException) {
        Assert.IsTrue(true);
      }
    }
    
    ////////////////////////////////

    // Revert console output to stdout.
    Console.SetOut(StdOut);
    
    // Delete File Created
    File.Delete(LogFile);
    
  }
  
  private static async Task TestLogger(Logger logger) {
    
    using (logger) {
      
      foreach (var item in logger.Levels) {
        
        await logger.LogAsync(item, SampleMessage);
        string expected = logger.Style.FormatEvent(item, logger.Name, SampleMessage);

        if (item.Print) {
          
          StringReader reader = new StringReader(StringWriter.ToString());
          string res = await reader.ReadToEndAsync();
          res = res.Replace("\n\r\n", "\n");
          Assert.AreEqual(expected, res);
          WipeStream();
          
        }
        
        if (item.Log && logger.StreamWriter is null)
          Assert.AreEqual(expected.Replace("\n", ""), File.ReadLines(LogFile).Last());

      }
      
      foreach (var item in logger.Levels) {
        
        await logger.LogAsync(item.Name, SampleMessage);
        string expected = logger.Style.FormatEvent(item, logger.Name, SampleMessage);

        if (item.Print) {
          
          StringReader reader = new StringReader(StringWriter.ToString());
          string res = await reader.ReadToEndAsync();
          res = res.Replace("\n\r\n", "\n");
          Assert.AreEqual(expected, res);
          WipeStream();
          
        }
        
        if (item.Log && logger.StreamWriter is null)
          Assert.AreEqual(expected.Replace("\n", ""), File.ReadLines(LogFile).Last());

      }
      
      if (logger.StreamWriter is not null) {
        
        logger.Dispose();
        StringBuilder expected = new StringBuilder();
        
        for (ushort i = 0; i < 2; i++) {
          
          foreach (var item in logger.Levels) {
          
            if (item.Log)
              expected.Append(logger.Style.FormatEvent(item, logger.Name, SampleMessage));

          }
          
        }
        
        Assert.AreEqual(expected.ToString(), await File.ReadAllTextAsync(LogFile));
        
      }
      
    }
    
  }
  
  private static void WipeStream() {
    StringWriter.Close();
    StringWriter = new StringWriter();
    Console.SetOut(StringWriter);
  }
  
}