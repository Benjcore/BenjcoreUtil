using BenjcoreUtil.Logging;

namespace UnitTests.Logging;

public class LoggerUnitTests
{
    [Fact]
    public void Logger_Log_ThrowsOnInvalidLogLevel()
    {
        // Arrange
        var logger = new Logger
        (
            new LogLevel[] { new("INFO", 0, LogSettings.Nothing) },
            LogStyle.SampleLogStyleYMD,
            null
        );
        var invalidLogLevel = new LogLevel("DEBUG", 1, LogSettings.Nothing);
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => logger.Log(invalidLogLevel, "Test"));
        Assert.Throws<ArgumentException>(() => logger.Log("DEBUG", "Test"));
    }
    
    [Fact]
    public void Logger_ThrowsOnEmptyLogLevelArray()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>
        (
            () => new Logger(Array.Empty<LogLevel>(), LogStyle.SampleLogStyleYMD, null)
        );
    }
    
    [Fact]
    public void Logger_ThrowsOnDuplicateLogLevelNames()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>
        (
            () => new Logger
            (
                new LogLevel[]
                {
                    new("INFO", 0, LogSettings.Nothing),
                    new("INFO", 1, LogSettings.Nothing)
                },
                LogStyle.SampleLogStyleYMD,
                null
            )
        );
    }
    
    [Fact]
    public void Logger_ThrowsOnDuplicateSeverityLevels()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>
        (
            () => new Logger
            (
                new LogLevel[]
                {
                    new("INFO", 0, LogSettings.Nothing),
                    new("DEBUG", 0, LogSettings.Nothing)
                },
                LogStyle.SampleLogStyleYMD,
                null
            )
        );
    }
}