using System.Reflection;
using FluentAssertions;
using Heyer.Storage.API.Providers.Storage.Filesystem;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Heyer.Storage.API.Tests.UnitTests.Providers.Filesystem;

public class FilesystemStorageStrategyTests
{
    private FilesystemStorageStrategy _strategy;
    private FilesystemStorageOptions _options;

    [SetUp]
    public void Setup()
    {
        var testRootPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, 
            "testRootPath");
        
        _options = new FilesystemStorageOptions()
        {
            RootPath = testRootPath
        };

        _strategy = new FilesystemStorageStrategy(Options.Create(_options), new NullLogger<FilesystemStorageStrategy>());
    }
    
    [TearDown]
    public void TearDown()
    {
        Directory.Delete(_options.RootPath, true);
    }

    [Test]
    public async Task StoreAsync_WhenInvokedWithStreamAndKey_ShouldStoreStream()
    {
        // Arrange
        var key = "test-key";
        var stream = new MemoryStream("test-data"u8.ToArray());

        // Act
        await _strategy.StoreAsync(key, stream);

        // Assert
        var filePath = Path.Combine(_options.RootPath, key);
        Assert.True(File.Exists(filePath));
        Assert.That(await File.ReadAllTextAsync(filePath), Is.EqualTo("test-data"));
    }

    [Test]
    public async Task StoreAsync_WhenInvokedMultipleTimesWithTheSameKey_ShouldReturnError()
    {
        // Arrange
        var key = "test-key";
        var stream1 = new MemoryStream("test-data1"u8.ToArray());
        var stream2 = new MemoryStream("test-data2"u8.ToArray());
        
        await _strategy.StoreAsync(key, stream1);

        // Act
        var result = await _strategy.StoreAsync(key, stream2);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("File already exists.");
        
        var filePath = Path.Combine(_options.RootPath, key);
        Assert.That(await File.ReadAllTextAsync(filePath), Is.EqualTo("test-data1"));
    }

    [Test]
    public async Task DeleteAsync_WhenInvokedWithKey_ShouldDeleteFile()
    {
        // Arrange
        var key = "test-key";
        var stream = new MemoryStream("test-data"u8.ToArray());
        await _strategy.StoreAsync(key, stream);

        // Act
        await _strategy.DeleteAsync(key);

        // Assert
        var filePath = Path.Combine(_options.RootPath, key);
        Assert.False(File.Exists(filePath));
    }

    [Test]
    public async Task GetAsync_WhenInvokedWithKey_ShouldReturnStream()
    {
        // Arrange
        var key = "test-key";
        var stream = new MemoryStream("test-data"u8.ToArray());
        await _strategy.StoreAsync(key, stream);

        // Act
        var result = await _strategy.GetAsync(key);

        // Assert
        result.IsSuccess.Should().BeTrue();
        
        var resultText = await GetTextFromStreamAsync(result.Value);
        Assert.That(resultText, Is.EqualTo("test-data"));
    }

    [Test]
    public async Task GetAsync_WhenKeyNotFound_ShouldReturnNotFoundError()
    {
        // Act
        var result = await _strategy.GetAsync("non-existing-key");
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("Not found.");
    }
    
    private static async Task<string> GetTextFromStreamAsync(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin); // Ensure the stream position is at the beginning
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }
}