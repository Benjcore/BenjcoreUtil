namespace BenjcoreUtil.Logging; 

/// <summary>
/// LogSettings is an enum used to configure whether or not a <see cref="LogLevel"/> 
/// should be logged to a file, printed to console (stdout), both or neither.
/// <p>
/// <br/>
/// <b>Possibilities:</b><br/>
/// PrintAndLog<br/>
/// JustLog<br/>
/// JustPrint<br/>
/// Nothing<br/>
/// </p>
/// </summary>
/// <remarks>
/// Note that 'Log' refers to logging to a file.
/// </remarks>
public enum LogSettings {
  PrintAndLog,
  JustLog,
  JustPrint,
  Nothing
}