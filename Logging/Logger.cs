using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BenjcoreUtil.Logging; 

/// <summary>
/// A BenjcoreUtil Logger object used to log events to a file and/or stdout.
/// </summary>
public class Logger : IDisposable {
  
  /// <summary>
  /// A <see cref="StreamWriter"/> that can be used to write to the <see cref="LogFile"/>.
  /// If null, <see cref="File">System.IO.File</see> will be used instead.
  /// </summary>
  public StreamWriter? StreamWriter { get; } = null;

  /// <summary>
  /// An Array of <see cref="LogLevel">LogLevels</see> that this <see cref="Logger"/> object can use.
  /// </summary>
  public LogLevel[] Levels { get; }
  
  /// <summary>
  /// The <see cref="LogStyle"/> that this <see cref="Logger"/> object should use.
  /// </summary>
  public LogStyle Style { get; }
  
  /// <summary>
  /// A string representing the path of the file that the <see cref="Logger"/> should output to.
  /// </summary>
  public string LogFile { get; }
  
  /// <summary>
  /// A string representing the name of the logger which can be referenced by <see cref="LogStyle"/>.
  /// </summary>
  public string Name { get; }
  
  /// <summary>
  /// Creates a new <see cref="Logger"/> instance.
  /// </summary>
  /// <param name="levels">
  /// A <see cref="LogLevel"/> array that dictates the possible <see cref="LogLevel">LogLevels</see> that this
  /// <see cref="Logger"/> can use. The array must contain at least one item and have no duplicate severities
  /// or names (case insensitive).</param>
  /// <param name="style">The <see cref="LogStyle"/> to use.</param>
  /// <param name="logFile">
  /// A string representing the path of the file that the <see cref="Logger"/> should output to.
  /// </param>
  /// <param name="name">
  /// A string representing the name of the logger which can be referenced by <see cref="LogStyle"/>. Default: "Logger"
  /// </param>
  /// <param name="singleStream">
  /// If true, this object will have an open <see cref="System.IO.StreamWriter"/> until it goes out of scope.
  /// The benefit of this is that a new IO stream will not need to be opened every time <see cref="Log(LogLevel,string)"/> or
  /// <see cref="LogAsync(LogLevel,string)"/> is called, resulting in a performance boost. The downside is that <paramref name="logFile"/>
  /// will not be able to be used by other processes and parts of your code. (Which may result in undesired errors.)
  /// If false, <see cref="Log(LogLevel,string)"/> and <see cref="LogAsync(LogLevel,string)"/> will use <see cref="System.IO.File"/> instead, which comes
  /// with a performance penalty as it opens and closes a new IO stream every time it is called. If you are unsure,
  /// stick with the default (false).
  /// </param>
  /// <exception cref="ArgumentException"><paramref name="levels"/> is empty or has duplicate severities or names.</exception>
  public Logger(LogLevel[] levels, LogStyle style, string logFile, string? name = "Logger", bool singleStream = false) {
    
    Levels = levels;
    Style = style;
    LogFile = logFile;
    Name = name ?? "Logger";
    
    // Create a list of level severities.
    List<int> levelSeverities = new();
    foreach (var item in Levels) levelSeverities.Add(item.Severity);
    
    // Create a list of level names.
    List<string> levelNames = new();
    foreach (var item in Levels) levelNames.Add(item.Name.ToUpper());
    
    // Ensure there is at least one LogLevel.
    if (levels.Length < 1)
      throw new ArgumentException("Empty array detected.", nameof(levels));
    
    // Using the list of level severities,
    // ensure all levels are of a different severity level.
    if (levelSeverities.Distinct().Count() != levelSeverities.Count)
      throw new ArgumentException("Duplicate severity values detected.", nameof(levels));
    
    // Using the list of level severities,
    // ensure all levels have different names.
    if (levelNames.Distinct().Count() != levelNames.Count)
      throw new ArgumentException("Duplicate level names detected.", nameof(levels));

    // Delete logFile if it already exists.
    if (File.Exists(logFile)) File.Delete(logFile);

    // Initialize StreamWriter if singleStream is true,
    // else it will stay as null.
    if (singleStream) {
      StreamWriter = new StreamWriter(logFile);
    }

  }
  
  /// <summary>
  /// Logs an event in the <see cref="Logger"/>.
  /// </summary>
  /// <param name="levelName">The string <see cref="LogLevel.Name"/> of the event's <see cref="LogLevel"/> (case insensitive).
  /// The <see cref="LogLevel"/> determines whether or not the event is logged and/or printed.</param>
  /// <param name="message">A string message that details the event.</param>
  /// <exception cref="ArgumentException"><paramref name="levelName"/> is not present in <see cref="Levels"/>.</exception>
  public void Log(string levelName, string message) {
    
    LogLevel level;
    
    try {
      
      // Figure out LogLevel based on levelName.
      level = (from item in Levels
               where String.Equals(item.Name, levelName,
                 StringComparison.CurrentCultureIgnoreCase)
               select item).First();
      
    } catch (InvalidOperationException e) {
      throw new ArgumentException($"Could not find LogLevel with the name '{levelName}'.", nameof(levelName), e);
    }
    
    // Call Log(LogLevel, string)
    Log(level, message);
    
  }
  
  /// <summary>
  /// Logs an event in the <see cref="Logger"/>.
  /// </summary>
  /// <param name="level">The <see cref="LogLevel"/> of the event. The <see cref="LogLevel"/>
  /// determines whether or not the event is logged and/or printed.</param>
  /// <param name="message">A string message that details the event.</param>
  /// <exception cref="ArgumentException"><paramref name="level"/> is not present in <see cref="Levels"/>.</exception>
  public void Log(LogLevel level, string message) {

    // Ensure level is in Levels
    foreach (var item in Levels) {
      
      if (item.GetHashCode() == level.GetHashCode()) break; // Break if level is in Levels.
      
      // If the loop reaches the end and
      // hasn't found level.
      if (item.Equals(Levels[^1])) {
        throw new ArgumentException($"{level.Name} is not in {Name}.{nameof(Levels)}", nameof(level));
      }
      
    }
    
    string? formattedMessage = null;
    
    // Call level Function
    if (level.Function is not null) {
      
      if (level.RawStringFunctionInvoke)
        level.Function.Invoke(message);
      else {
        formattedMessage = Style.FormatEvent(level, Name, message);
        level.Function.Invoke(formattedMessage);
      }
      
      
    }

    // Return early if Log and Print are false.
    if (!level.Log && !level.Print) return;
    
    if (String.IsNullOrEmpty(formattedMessage))
      formattedMessage = Style.FormatEvent(level, Name, message);
    
    if (level.Log) {
      
      if (StreamWriter is null) {
        
        // Append to LogFile
        File.AppendAllText(LogFile, formattedMessage);
        
      } else {
        
        // Append to LogFile via StreamWriter
        StreamWriter.WriteAsync(formattedMessage);
        
      }
      
    }
    
    if (level.Print) {
      
      Console.WriteLine(formattedMessage);
      
    }
    
  }
  
  /// <summary>
  /// Asynchronously logs an event in the <see cref="Logger"/>.
  /// </summary>
  /// <param name="level"><inheritdoc cref="Log(LogLevel,string)"/></param>
  /// <param name="message"><inheritdoc cref="Log(LogLevel,string)"/></param>
  /// <exception cref="ArgumentException"><inheritdoc cref="Log(LogLevel,string)"/></exception>
  public async Task LogAsync(LogLevel level, string message) {
    
    // Create a task that runs Log().
    var task = new Task(delegate { Log(level, message); });
    task.Start(); // Start Task
    await task; // Await Completion
    
  }
  
  /// <summary>
  /// Asynchronously logs an event in the <see cref="Logger"/>.
  /// </summary>
  /// <param name="levelName"><inheritdoc cref="Log(string,string)"/></param>
  /// <param name="message"><inheritdoc cref="Log(string,string)"/></param>
  /// <exception cref="ArgumentException"><inheritdoc cref="Log(string,string)"/></exception>
  public async Task LogAsync(string levelName, string message) {
    
    // Create a task that runs Log().
    var task = new Task(delegate { Log(levelName, message); });
    task.Start(); // Start Task
    await task; // Await Completion
    
  }
  
  //
  // IDisposable Implementation
  //
  
  [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
  ~Logger() {
    Dispose(false);
  }

  private void ReleaseUnmanagedResources() {
    StreamWriter?.Close();
  }

  private void Dispose(bool disposing) {
    ReleaseUnmanagedResources();
    if (disposing) {
      StreamWriter?.Dispose();
    }
  }

  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
  
}