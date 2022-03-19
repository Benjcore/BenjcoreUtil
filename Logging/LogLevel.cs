using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BenjcoreUtil.Logging; 

/// <summary>
/// A struct used to represent a <see cref="Logger"/> level.
/// </summary>
public struct LogLevel {

  /// <summary>
  /// The name of the <see cref="LogLevel"/> to be displayed.
  /// </summary>
  public string Name { get; }

  /// <summary>
  /// An integer representation of the <see cref="LogLevel"/>'s severity with the lowest
  /// being most severe and the highest being the least severe.
  /// </summary>
  /// <remarks>
  /// Using a lower number to represent a more critical error is recommended because
  /// it is more common to need to add a less severe <see cref="LogLevel"/> rather than
  /// a more-severe one. (Especially if the most-severe level already means fatal.)
  /// </remarks>
  public int Severity { get; }
  
  /// <summary>
  /// A boolean representing whether or not an event of this level should be printed to console (stdout).
  /// </summary>
  public bool Print { get; set; }
  
  /// <summary>
  /// A boolean representing whether or not an event of this level should be
  /// written to the <see cref="Logger"/>'s <see cref="Logger.LogFile"/>.
  /// </summary>
  public bool Log { get; set; }

  /// <summary>
  /// Represents a <see cref="Action"/> that is executed whenever a <see cref="Logger"/> logs
  /// an event of the current <see cref="LogLevel"/>. The <see cref="Action"/> must take a
  /// string parameter which represents the log message of the event.
  /// </summary>
  /// <remarks>
  /// This feature is intended as a way to automatically log events in external ways without having
  /// to log a single event multiple times. This may used to copy information of an event to something
  /// like a dialog box or Debug.Log() in unity.
  /// </remarks>
  public Action<string>? Function { get; } = null;
  
  /// <summary>
  /// If true, the <see cref="Logger"/> will use the raw event message as the <see cref="Function"/>
  /// parameter. If false, the <see cref="Logger"/> will use the event message formatted by
  /// <see cref="Logger.Style">Logger.Style</see>.
  /// </summary>
  public bool RawStringFunctionInvoke { get; } = false;

  /// <summary>
  /// Reads a <see cref="LogSettings"/> enum and creates a <see cref="Tuple{T1, T2}">Tuple&#60;bool, bool&#62;</see>
  /// representation of it.
  /// </summary>
  /// <param name="settings"><see cref="LogSettings"/> you wish to read.</param>
  /// <returns>A <see cref="Tuple{T1, T2}">Tuple&#60;bool, bool&#62;</see> representation of <paramref name="settings"/>
  /// where Item1 represents <see cref="Log"/> and Item2 represents <see cref="Print"/>.</returns>
  /// <exception cref="InvalidEnumArgumentException">Invalid enum value in <paramref name="settings"/>.</exception>
  private static Tuple<bool, bool> ReadLogSettings(LogSettings settings) {
    switch (settings) {
      
      case LogSettings.PrintAndLog:
        return Tuple.Create<bool, bool>(true, true);
      
      case LogSettings.JustLog:
        return Tuple.Create<bool, bool>(true, false);
      
      case LogSettings.JustPrint:
        return Tuple.Create<bool, bool>(false, true);
      
      case LogSettings.Nothing:
        return Tuple.Create<bool, bool>(false, false);
      
      default:
        throw new InvalidEnumArgumentException("Input is invalid.");
      
    }
  }

  /// <summary>
  /// Creates new a new instance of the <see cref="LogLevel"/> class.
  /// </summary>
  /// <param name="name"><see cref="Name"/></param>
  /// <param name="severity"><see cref="Severity"/></param>
  /// <param name="logSettings">A <see cref="LogSettings"/> enum that
  /// dictates <see cref="Log"/> and <see cref="Print"/>.</param>
  /// <exception cref="InvalidEnumArgumentException">Invalid enum value in <paramref name="logSettings"/>.</exception>
  public LogLevel(string name, int severity, LogSettings logSettings) {
    Name = name;
    Severity = severity;
    
    // Set Log & Print
    var settings = ReadLogSettings(logSettings);
    Log = settings.Item1;
    Print = settings.Item2;
  }

  /// <summary>
  /// Creates new a new instance of the <see cref="LogLevel"/> class.
  /// </summary>
  /// <param name="name"><see cref="Name"/></param>
  /// <param name="severity"><see cref="Severity"/></param>
  /// <param name="function"><see cref="Function"/></param>
  /// <param name="logSettings">A <see cref="LogSettings"/> enum that
  /// dictates <see cref="Log"/> and <see cref="Print"/>.</param>
  /// <param name="funcRawMsg"><see cref="RawStringFunctionInvoke"/></param>
  /// <exception cref="InvalidEnumArgumentException">Invalid enum value in <paramref name="logSettings"/>.</exception>
  public LogLevel(string name, int severity, LogSettings logSettings, Action<string> function, bool funcRawMsg) {
    Name = name;
    Severity = severity;
    Function = function;
    RawStringFunctionInvoke = funcRawMsg;
    
    // Set Log & Print
    var settings = ReadLogSettings(logSettings);
    Log = settings.Item1;
    Print = settings.Item2;
  }

  /// <summary>
  /// Returns a string that represents the current object.
  /// </summary>
  /// <returns><see cref="Name"/></returns>
  public override string ToString() {
    return Name;
  }

  public override int GetHashCode() {
    return Name.GetHashCode() / 2 + Severity;
  }

  public override bool Equals(object? obj) {
    
    if (obj is LogLevel level) {
      return Severity == level.Severity;
    }

    return RuntimeHelpers.Equals(this, obj);
    
  }

  public static implicit operator LogLevel(Tuple<string, int, LogSettings> input) =>
    new(input.Item1, input.Item2, input.Item3);

  public static bool operator <(LogLevel a, LogLevel b) => a.Severity < b.Severity;
  public static bool operator >(LogLevel a, LogLevel b) => a.Severity > b.Severity;
  public static bool operator <=(LogLevel a, LogLevel b) => a.Severity <= b.Severity;
  public static bool operator >=(LogLevel a, LogLevel b) => a.Severity >= b.Severity;
  public static bool operator ==(LogLevel a, LogLevel b) => a.Severity == b.Severity;
  public static bool operator !=(LogLevel a, LogLevel b) => a.Severity != b.Severity;

}