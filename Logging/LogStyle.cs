using System.Text;

namespace BenjcoreUtil.Logging;

/// <summary>
/// A struct that represents how a <see cref="Logger"/> object should format logged events.<br/><br/>
/// <p>
/// The LogStyle datatype consists of a string. Regular characters will be logged as-is
/// but to use special functions you can use the <see cref="EscapeCharacter"/> followed
/// by another character (see below) or use the <see cref="EscapeCharacter"/> twice if
/// you want to print the <see cref="EscapeCharacter"/> as a normal character.<br/><br/>
///
/// <b>Escape Functions:</b><br/><br/>
///
/// <b>L :</b><br/>
/// &#009;The <see cref="LogLevel"/> of the event.<br/><br/>
///
/// <b>N :</b><br/>
/// &#009;The name of the <see cref="Logger"/> object.<br/><br/>
///
/// <b>T :</b><br/>
/// &#009;Represents the current date and/or time. It should be followed by a string (without quotes) surround
/// by braces '{ }'. The string will be passed to the <see cref="DateTime.ToString(string?)">
/// DateTime.Now.ToString(string?)</see> method to format a <see cref="DateTime"/> which represents the current time.
/// 
/// </p>
/// </summary>
/// <remarks>
/// The character that follows an <see cref="EscapeCharacter"/> is case-insensitive.
/// </remarks>
/// <example>
/// <c>"[%l] %t{yy/MM/dd HH:mm:ss} : "</c> may output :<br/>
/// <c>"[INFO] 98/01/05 16:05:35 : *Log Event Message*"</c>
/// </example>
public readonly struct LogStyle
{
    /// <summary>
    /// A sample <see cref="LogStyle"/> using the metric Day / Month / Year format.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    public static LogStyle SampleLogStyleDMY { get; } = new("[%l] %t{dd/MM/yy HH:mm:ss} : ");

    /// <summary>
    /// A sample <see cref="LogStyle"/> using the imperial Month / Day / Year format.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    public static LogStyle SampleLogStyleMDY { get; } = new("[%l] %t{MM/dd/yy HH:mm:ss} : ");

    /// <summary>
    /// A sample <see cref="LogStyle"/> using the universal Year / Month / Day format.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute]
    public static LogStyle SampleLogStyleYMD { get; } = new("[%l] %t{yy/MM/dd HH:mm:ss} : ");

    /// <summary>
    /// Represents the escape character to use for <see cref="LogStyle"/> formatting.
    /// By default this is '%'.
    /// </summary>
    internal const char EscapeCharacter = '%';

    /// <summary>
    /// The internal character array representation of the string used to create this <see cref="LogStyle"/> object.
    /// </summary>
    internal readonly char[] Data;

    /// <summary>
    /// Creates a new instance of the <see cref="LogStyle"/> datatype.
    /// </summary>
    /// <param name="format">A string that dictates the format of the new <see cref="LogStyle"/> struct. 
    /// Formatting information can be found <see cref="LogStyle">here</see>.</param>
    /// <exception cref="FormatException"><paramref name="format"/> is invalid.</exception>
    public LogStyle(string format)
    {
        Data = format.ToCharArray();

        // Try GetString() to ensure format is valid.
        // If not, a FormatException will be thrown.
        {
            LogLevel test = Tuple.Create<string, int, LogSettings>("Test", Int16.MaxValue, LogSettings.Nothing);
            var dummy = FormatEvent(test, "Test-Logger", "This is a test event.");
        }
    }

    /// <summary>
    /// Generates a formatted <see cref="Logger"/> event using this <see cref="LogStyle"/> instance.
    /// </summary>
    /// <param name="level">The name of the <see cref="LogLevel"/> of the event.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <param name="logEvent">The string body of the event.</param>
    /// <param name="dateTime">The date and time that the event occured. If null, it will default to <see cref="DateTime.Now"/>.</param>
    /// <returns>The formatted string output.</returns>
    /// <exception cref="FormatException"><see cref="Data"/> is invalid.</exception>
    public string FormatEvent(string level, string loggerName, string logEvent, DateTime? dateTime)
    {
        DateTime givenDateTime = dateTime ?? DateTime.Now;
        StringBuilder sb = new StringBuilder();

        for (uint i = 0; i < Data.Length; i++)
        {
            if (Data[i] is EscapeCharacter)
            {
                // Increment i by one to check the next char.
                i++;

                try
                {
                    switch (Char.Parse(Data[i].ToString().ToLower()))
                    {
                        case EscapeCharacter:
                            sb.Append(EscapeCharacter);
                            break;

                        case 'l':
                            sb.Append(level);
                            break;

                        case 'n':
                            sb.Append(loggerName);
                            break;

                        case 't':
                        {
                            // Check next char to ensure it's '{'
                            i++;

                            if (Data[i] is not '{')
                                throw new FormatException($"Expected '{{' at char {i + 1}.");

                            i++;
                            StringBuilder sb2 = new StringBuilder();

                            while (Data[i] is not '}')
                            {
                                sb2.Append(Data[i]);
                                i++;
                            }

                            sb.Append(givenDateTime.ToString(sb2.ToString()));

                            break;
                        }


                        default:
                            throw new FormatException($"Unknown escape sequence: '{EscapeCharacter}{Data[i]}'");
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    throw new FormatException("Unexpected end of format string.");
                }
            }
            else
            {
                sb.Append(Data[i]);
            }
        }

        sb.Append(logEvent);
        sb.Append('\n');
        return sb.ToString();
    }
    
    /// <summary>
    /// Generates a formatted <see cref="Logger"/> event using this <see cref="LogStyle"/> instance.
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/> of the event.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <param name="logEvent">The string body of the event.</param>
    /// <param name="dateTime">The date and time that the event occured. If null, it will default to <see cref="DateTime.Now"/>.</param>
    /// <returns>The formatted string output.</returns>
    /// <exception cref="FormatException"><see cref="Data"/> is invalid.</exception>
    public string FormatEvent(LogLevel level, string loggerName, string logEvent, DateTime? dateTime)
    {
        return FormatEvent(level.Name, loggerName, logEvent, dateTime);
    }
    
    /// <summary>
    /// Generates a formatted <see cref="Logger"/> event using this <see cref="LogStyle"/> instance.
    /// </summary>
    /// <param name="level">The name of the <see cref="LogLevel"/> of the event.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <param name="logEvent">The string body of the event.</param>
    /// <returns>The formatted string output.</returns>
    /// <exception cref="FormatException"><see cref="Data"/> is invalid.</exception>
    public string FormatEvent(string level, string loggerName, string logEvent)
    {
        return FormatEvent(level, loggerName, logEvent, null);
    }
    
    /// <summary>
    /// Generates a formatted <see cref="Logger"/> event using this <see cref="LogStyle"/> instance.
    /// </summary>
    /// <param name="level">The <see cref="LogLevel"/> of the event.</param>
    /// <param name="loggerName">The name of the logger.</param>
    /// <param name="logEvent">The string body of the event.</param>
    /// <returns>The formatted string output.</returns>
    /// <exception cref="FormatException"><see cref="Data"/> is invalid.</exception>
    public string FormatEvent(LogLevel level, string loggerName, string logEvent)
    {
        return FormatEvent(level.Name, loggerName, logEvent, null);
    }

    public static implicit operator LogStyle(string input) => new(input);
}