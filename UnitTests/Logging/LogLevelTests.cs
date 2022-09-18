using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using BenjcoreUtil.Logging;

namespace UnitTests.Logging;

[SuppressMessage("ReSharper", "EqualExpressionComparison")]
public class LogLevelTests
{
    /// <summary>
    /// Represents a more urgent log level.
    /// </summary>
    private static readonly LogLevel UrgentLevel = new LogLevel("Urgent", 0, LogSettings.Nothing);
    
    /// <summary>
    /// Represents a less urgent log level.
    /// </summary>
    private static readonly LogLevel ChillLevel = new LogLevel("Chill", 1, LogSettings.Nothing);
    
    /*
     * Because these test are very simple, we don't
     * need to follow the arrange/act/assert pattern.
     */
    
#region Comparasons
    
    [Fact]
    public void LogLevel_EqualsOperator_ReturnsTrueWhenEqual()
    {
        Assert.True(ChillLevel == ChillLevel);
        Assert.True(UrgentLevel == UrgentLevel);
    }
    
    [Fact]
    public void LogLevel_EqualsOperator_ReturnsFalseWhenNotEqual()
    {
        Assert.False(ChillLevel == UrgentLevel);
    }
    
    [Fact]
    public void LogLevel_EqualsMethod_ReturnsTrueWhenEqual()
    {
        Assert.True(ChillLevel.Equals(ChillLevel));
        Assert.True(UrgentLevel.Equals(UrgentLevel));
    }
    
    [Fact]
    public void LogLevel_EqualsMethod_ReturnsFalseWhenNotEqual()
    {
        Assert.False(ChillLevel.Equals(UrgentLevel));
    }
    
    [Fact]
    public void LogLevel_EqualsMethod_ReturnsFalseOnWrongType()
    {
        Assert.False(ChillLevel.Equals(new object()));
    }
    [Fact]
    public void LogLevel_EqualsMethod_ReturnsFalseOnNull()
    {
        Assert.False(ChillLevel.Equals(null));
    }
    
    [Fact]
    public void LogLevel_NotEqualOperator_ReturnsFalseWhenEqual()
    {
        Assert.False(ChillLevel != ChillLevel);
        Assert.False(UrgentLevel != UrgentLevel);
    }
    
    [Fact]
    public void LogLevel_NotEqualOperator_ReturnsTrueWhenNotEqual()
    {
        Assert.True(ChillLevel != UrgentLevel);
    }
    
    [Fact]
    public void LogLevel_LessThanOperator_ReturnsTrueWhenCurrentIsMoreSevereThanInput()
    {
        Assert.True(UrgentLevel < ChillLevel);
    }
    
    [Fact]
    public void LogLevel_LessThanOperator_ReturnsFalseWhenCurrentIsEqualToInput()
    {
        Assert.False(ChillLevel < ChillLevel);
    }
    
    [Fact]
    public void LogLevel_LessThanOperator_ReturnsFalseWhenCurrentIsLessSevereThanInput()
    {
        Assert.False(ChillLevel < UrgentLevel);
    }

    [Fact]
    public void LogLevel_LessThanOrEqualOperator_ReturnsTrueWhenCurrentIsMoreSevereThanInput()
    {
        Assert.True(UrgentLevel <= ChillLevel);
    }
    
    [Fact]
    public void LogLevel_LessThanOrEqualOperator_ReturnsTrueWhenCurrentIsEqualToInput()
    {
        Assert.True(ChillLevel <= ChillLevel);
    }
    
    [Fact]
    public void LogLevel_LessThanOrEqualOperator_ReturnsFalseWhenCurrentIsLessSevereThanInput()
    {
        Assert.False(ChillLevel <= UrgentLevel);
    }
    
    [Fact]
    public void LogLevel_GreaterThanOperator_ReturnsFalseWhenCurrentIsMoreSevereThanInput()
    {
        Assert.False(UrgentLevel > ChillLevel);
    }
    
    [Fact]
    public void LogLevel_GreaterThanOperator_ReturnsFalseWhenCurrentIsEqualToInput()
    {
        Assert.False(ChillLevel > ChillLevel);
    }
    
    [Fact]
    public void LogLevel_GreaterThanOperator_ReturnsTrueWhenCurrentIsLessSevereThanInput()
    {
        Assert.True(ChillLevel > UrgentLevel);
    }
    
    [Fact]
    public void LogLevel_GreaterThanOrEqualOperator_ReturnsFalseWhenCurrentIsMoreSevereThanInput()
    {
        Assert.False(UrgentLevel >= ChillLevel);
    }
    
    [Fact]
    public void LogLevel_GreaterThanOrEqualOperator_ReturnsTrueWhenCurrentIsEqualToInput()
    {
        Assert.True(ChillLevel >= ChillLevel);
    }
    
    [Fact]
    public void LogLevel_GreaterThanOrEqualOperator_ReturnsTrueWhenCurrentIsLessSevereThanInput()
    {
        Assert.True(ChillLevel >= UrgentLevel);
    }
    
#endregion
    
    [Fact]
    public void LogLevel_ToString_ReturnsName()
    {
        Assert.Equal("Chill", ChillLevel.ToString());
        Assert.Equal(ChillLevel.Name, ChillLevel.ToString());
        Assert.Equal("Urgent", UrgentLevel.ToString());
        Assert.Equal(UrgentLevel.Name, UrgentLevel.ToString());
    }
    
    [Fact]
    public void LogLevel_GetHashCode_ReturnsSameHashCodeForSameLevel()
    {
        Assert.Equal(ChillLevel.GetHashCode(), ChillLevel.GetHashCode());
        Assert.Equal(UrgentLevel.GetHashCode(), UrgentLevel.GetHashCode());
    }
    
    [Fact]
    public void LogLevel_GetHashCode_ReturnsDifferentHashCodeForDifferentLevel()
    {
        Assert.NotEqual(ChillLevel.GetHashCode(), UrgentLevel.GetHashCode());
    }
    
    [Fact]
    public void LogLevel_ImplicitConversionTest()
    {
        // Note: We don't need to assert anything here, we just need the conversion to succeed.
        LogLevel dummy = new Tuple<string, int, LogSettings>("Test", 0, LogSettings.Nothing);
    }
    
    [Fact]
    public void LogLevel_LogSettings()
    {
        var nothing = new LogLevel("Test", 0, LogSettings.Nothing);
        Assert.False(nothing.Log);
        Assert.False(nothing.Print);
        
        var log = new LogLevel("Test", 0, LogSettings.JustLog);
        Assert.True(log.Log);
        Assert.False(log.Print);
        
        var print = new LogLevel("Test", 0, LogSettings.JustPrint);
        Assert.False(print.Log);
        Assert.True(print.Print);
        
        var printAndLog = new LogLevel("Test", 0, LogSettings.PrintAndLog);
        Assert.True(printAndLog.Log);
        Assert.True(printAndLog.Print);
        
        Assert.Throws<InvalidEnumArgumentException>(() => new LogLevel("Test", 0 , (LogSettings)0xFF));
    }
    
    [Fact]
    public void LogLevel_Function_funcRawMsg_False()
    {
        // Arrange
        var logLevel = new LogLevel
            (
                "INFO",
                0,
                LogSettings.Nothing,
                a => throw new Exception(a),
                false
            );
        const string msg = "Hello World!";
        var style = LogStyle.SampleLogStyleYMD;
        var logger = new Logger
            (
                new LogLevel[] { logLevel },
                style, 
                null
            );

        // Act
        Exception? e = new Func<Exception?>(() =>
        {
            try
            {
                logger.Log(logLevel, msg);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }).Invoke();

        // Assert
        Assert.NotNull(e);
        Assert.Equal(style.FormatEvent(logLevel, "Logger", msg), e.Message);
    }
    
    [Fact]
    public void LogLevel_Function_funcRawMsg_True()
    {
        // Arrange
        var logLevel = new LogLevel
        (
            "INFO",
            0,
            LogSettings.Nothing,
            a => throw new Exception(a),
            true
        );
        const string msg = "Hello World!";
        var style = LogStyle.SampleLogStyleYMD;
        var logger = new Logger
        (
            new LogLevel[] { logLevel },
            style, 
            null
        );

        // Act
        Exception? e = new Func<Exception?>(() =>
        {
            try
            {
                logger.Log(logLevel, msg);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }).Invoke();

        // Assert
        Assert.NotNull(e);
        Assert.Equal(msg, e.Message);
    }
}