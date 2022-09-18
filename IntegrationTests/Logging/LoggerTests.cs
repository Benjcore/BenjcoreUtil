using BenjcoreUtil.Logging;

namespace IntegrationTests.Logging;

[Collection("Log To File Integration Tests")] // Don't run in parallel
public class LoggerTests
{
    private const string FileDirectory = "FileSystemLogs";
    private const string ArchiveDirectory = "LogArchives";

    static LoggerTests()
    {
        Cleanup();
        
        // The log archive allows us to view
        // the logs from the previous run.
        if (Directory.Exists(ArchiveDirectory))
        {
            Directory.Delete(ArchiveDirectory, true /* Recursive */);
        }
        Directory.CreateDirectory(ArchiveDirectory);
    }
    
    private static void Cleanup()
    {
        // Delete file system testing
        // directory if it exists.
        if (Directory.Exists(FileDirectory))
        {
            Directory.Delete(FileDirectory, true /* Recursive */);
        }
        Directory.CreateDirectory(FileDirectory);
    }
    
    private static void Archive(string? file)
    {
        if (file is null) return;
        File.Copy($"./{FileDirectory}/{file}", $"./{ArchiveDirectory}/{file}");
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void LoggerTests_LogFileNotCreatedWhenLogFileIsNull(bool singleStream)
    {
        // Arrange
        Cleanup();
        
        // Act
        using var logger = new Logger
        (
            new LogLevel[] { new("Test", 0, LogSettings.Nothing) },
            LogStyle.SampleLogStyleYMD,
            null,
            singleStream: singleStream
        );
        logger.Log("Test", "Sample Text");
        
        // Assert
        Assert.Empty(Directory.GetFiles($"./{FileDirectory}"));
    }
    
    [Fact]
    public void LoggerTests_DeletesLogFileIfItExists()
    {
        // Arrange
        Cleanup();
        const string fileName = $"./{FileDirectory}/Test.log";
        var stream = File.Create(fileName);
        stream.Write(new byte[] { 0xFF , 0xEE , 0xFF , 0xDD });
        stream.Dispose();
        
        // Act
        using var logger = new Logger
        (
            new LogLevel[] { new("Test", 0, LogSettings.Nothing) },
            LogStyle.SampleLogStyleYMD,
            fileName,
            singleStream: true
        );
        logger.Log("Test", "Sample Text");
        logger.Dispose();

        // Assert
        Assert.Empty(File.ReadAllBytes(fileName));
    }
    
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public void LoggerTests_LogToFileTest(bool singleStream, bool stringLogLevel)
    {
        // Redirect Standard Output
        var standardOutput = Console.Out;
        StringWriter consoleOutput = new();
        Console.SetOut(consoleOutput);
        
        try
        {
            // Arrange
            Cleanup();
            var logLevels = new LogLevel[]
            {
                new("Nothing", 3, LogSettings.Nothing),
                new("Log", 2, LogSettings.JustLog),
                new("Print", 1, LogSettings.JustPrint),
                new("PrintAndLog", 0, LogSettings.PrintAndLog)
            };
            var style = new LogStyle("[%n] [%l] %t{yyyy} : ");
            string year = DateTime.Now.ToString("yyyy");
            string logFileName = $"{Guid.NewGuid()}.log";
            string logFilePath = $"./{FileDirectory}/{logFileName}";
            const string name = "TestLogger";
            
            // Act - Create Logger
            using var logger = new Logger
            (
                logLevels,
                style,
                logFilePath,
                name: name,
                singleStream: singleStream
            );

            // Act - Log
            if (stringLogLevel)
            {
                logger.Log("Nothing", "Sample Text Nothing");
                logger.Log("Log", "Sample Text Log");
                logger.Log("Print", "Sample Text Print");
                logger.Log("PrintAndLog", "Sample Text PrintAndLog");
            }
            else
            {
                logger.Log(logLevels[0], "Sample Text Nothing");
                logger.Log(logLevels[1], "Sample Text Log");
                logger.Log(logLevels[2], "Sample Text Print");
                logger.Log(logLevels[3], "Sample Text PrintAndLog");
            }
            logger.Dispose();
            
            // Archive log file
            Archive(logFileName);
            
            // Assert
            using var consoleReader = new StringReader
            (
                consoleOutput
                .GetStringBuilder()
                .ToString()
                .ReplaceLineEndings()
                .Replace($"{Environment.NewLine}{Environment.NewLine}", Environment.NewLine)
            );
            Assert.Equal($"[{name}] [Print] {year} : Sample Text Print", consoleReader.ReadLine());
            Assert.Equal($"[{name}] [PrintAndLog] {year} : Sample Text PrintAndLog", consoleReader.ReadLine());
            
            using var fileReader = new StreamReader(logFilePath);
            Assert.Equal($"[{name}] [Log] {year} : Sample Text Log", fileReader.ReadLine());
            Assert.Equal($"[{name}] [PrintAndLog] {year} : Sample Text PrintAndLog", fileReader.ReadLine());
        }
        catch (Exception)
        {
            // Revert Standard Output
            Console.SetOut(standardOutput);
            throw;
        }
        finally
        {
            // Revert Standard Output
            Console.SetOut(standardOutput);
        }
    }
    
    [Theory]
    [InlineData(true, true)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public async Task LoggerTests_LogToFileTest_Async(bool singleStream, bool stringLogLevel)
    {
        // Redirect Standard Output
        var standardOutput = Console.Out;
        StringWriter consoleOutput = new();
        Console.SetOut(consoleOutput);
        
        try
        {
            // Arrange
            Cleanup();
            var logLevels = new LogLevel[]
            {
                new("Nothing", 3, LogSettings.Nothing),
                new("Log", 2, LogSettings.JustLog),
                new("Print", 1, LogSettings.JustPrint),
                new("PrintAndLog", 0, LogSettings.PrintAndLog)
            };
            var style = new LogStyle("[%n] [%l] %t{yyyy} : ");
            string year = DateTime.Now.ToString("yyyy");
            string logFileName = $"{Guid.NewGuid()}.log";
            string logFilePath = $"./{FileDirectory}/{logFileName}";
            const string name = "TestLogger";
            
            // Act - Create Logger
            using var logger = new Logger
            (
                logLevels,
                style,
                logFilePath,
                name: name,
                singleStream: singleStream
            );

            // Act - Log
            if (stringLogLevel)
            {
                await logger.LogAsync("Nothing", "Sample Text Nothing");
                await logger.LogAsync("Log", "Sample Text Log");
                await logger.LogAsync("Print", "Sample Text Print");
                await logger.LogAsync("PrintAndLog", "Sample Text PrintAndLog");
            }
            else
            {
                await logger.LogAsync(logLevels[0], "Sample Text Nothing");
                await logger.LogAsync(logLevels[1], "Sample Text Log");
                await logger.LogAsync(logLevels[2], "Sample Text Print");
                await logger.LogAsync(logLevels[3], "Sample Text PrintAndLog");
            }
            logger.Dispose();
            
            // Archive log file
            Archive(logFileName);
            
            // Assert
            using var consoleReader = new StringReader
            (
                consoleOutput
                .GetStringBuilder()
                .ToString()
                .ReplaceLineEndings()
                .Replace($"{Environment.NewLine}{Environment.NewLine}", Environment.NewLine)
            );
            Assert.Equal($"[{name}] [Print] {year} : Sample Text Print", await consoleReader.ReadLineAsync());
            Assert.Equal($"[{name}] [PrintAndLog] {year} : Sample Text PrintAndLog", await consoleReader.ReadLineAsync());
            
            using var fileReader = new StreamReader(logFilePath);
            Assert.Equal($"[{name}] [Log] {year} : Sample Text Log", await fileReader.ReadLineAsync());
            Assert.Equal($"[{name}] [PrintAndLog] {year} : Sample Text PrintAndLog", await fileReader.ReadLineAsync());
        }
        catch (Exception)
        {
            // Revert Standard Output
            Console.SetOut(standardOutput);
            throw;
        }
        finally
        {
            // Revert Standard Output
            Console.SetOut(standardOutput);
        }
    }
}