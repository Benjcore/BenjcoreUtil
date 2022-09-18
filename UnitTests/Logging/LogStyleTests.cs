using BenjcoreUtil.Logging;

namespace UnitTests.Logging;

public class LogStyleTests
{
    [Fact]
    public void LogStyle_EscTWithoutOpenBrace_Throws()
    {
        Assert.Throws<FormatException>(() =>
        {
#pragma warning disable CS0219
            LogStyle dummy = "%t}";
#pragma warning restore CS0219
        });
    }
    
    [Fact]
    public void LogStyle_EscTWithoutCloseBrace_Throws()
    {
        Assert.Throws<FormatException>(() =>
        {
#pragma warning disable CS0219
            LogStyle dummy = "%t{yy";
#pragma warning restore CS0219
        });
    }
    
    [Fact]
    public void LogStyle_InvalidEscSequence_Throws()
    {
        Assert.Throws<FormatException>(() =>
        {
#pragma warning disable CS0219
            LogStyle dummy = "%!{hello}";
#pragma warning restore CS0219
        });
    }
    
    [Theory]
    /*
     * LogLevel: INFO
     * LoggerName: Logger
     * Message: Hello World!
     * Year: 2025
     * Month: 12
     * Day: 31
     * Hour: 23
     * Minute: 59
     * Second: 59
     * (Expected results should also end of with a newline)
     */
    [InlineData("[%l] %t{dd/MM/yy HH:mm:ss} : ", "[INFO] 31/12/25 23:59:59 : Hello World!\n", false)]
    [InlineData("[%l] %t{MM/dd/yy HH:mm:ss} : ", "[INFO] 12/31/25 23:59:59 : Hello World!\n", false)]
    [InlineData("[%l] %t{yy/MM/dd HH:mm:ss} : ", "[INFO] 25/12/31 23:59:59 : Hello World!\n", false)]
    [InlineData("[%N] [%L] %T{yy/MM/dd HH:mm:ss} : ", "[Logger] [INFO] 25/12/31 23:59:59 : Hello World!\n", false)]
    [InlineData("", "Hello World!\n", false)]
    [InlineData("%% ", "% Hello World!\n", false)]
    [InlineData("%t{yy", "", true)]
    [InlineData("%t}", "", true)]
    [InlineData("%!{hello}", "", true)]
    public void LogStyle_TestCases(string input, string expected, bool shouldThrow)
    {
        // Arrange
        LogStyle style;

        if (shouldThrow)
        {
            // Act & Assert
            Assert.Throws<FormatException>(() =>
            {
                style = input;
            });
        }
        else
        {
            // Act
            style = input;
            string actual1 = style.FormatEvent("INFO", "Logger", "Hello World!", new DateTime(2025, 12, 31, 23, 59, 59));
            LogLevel infoLevel = new("INFO", 0, LogSettings.Nothing);
            string actual2 = style.FormatEvent(infoLevel, "Logger", "Hello World!", new DateTime(2025, 12, 31, 23, 59, 59));
        
            // Assert
            Assert.Equal(expected, actual1);
            Assert.Equal(expected, actual2);
        }
    }
    
    [Fact]
    public void LogStyle_FormatEvent_CorrectDateTime()
    {
        // Arrange
        string expected = $"[INFO] {DateTime.Now:yy/MM/dd HH:mm:ss} : Hello World!\n";
        
        // Act
        string actual = LogStyle.SampleLogStyleYMD.FormatEvent("INFO", "Logger", "Hello World!");
        
        // Assert
        Assert.Equal(expected, actual);
    }
}