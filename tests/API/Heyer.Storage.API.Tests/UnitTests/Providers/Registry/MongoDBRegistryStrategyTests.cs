using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Tests.Fixtures;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Registry.MongoDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Heyer.Storage.API.Tests.UnitTests.Providers.Registry;

[Category("Unit")]
[Ignore("MongoDB is abandoned")]
public class MongoDBRegistryStrategyTests
{
    private readonly MongoDbFixture _mongoDbFixture = new();
    private IMongoCollection<StorageRegistryEntry> _collection;
    private IDateTimeProvider _dateTimeProvider;
    private RegistryStrategyOptions _options;
    private DateTime _refDate;
    private MongoDBRegistryStrategy _strategy;

    [Test]
    public async Task DeleteAsync_ShouldDeleteExistingEntry()
    {
        // Arrange
        await InsertStorageEntry("test-key", "test-data"u8.ToArray(), "test-file.txt");

        // Act
        var result = await _strategy.DeleteAsync("test-key", CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var entry = await _collection.Find(x => x.Key == "test-key").SingleOrDefaultAsync();
        entry.Should().BeNull();
    }

    [Test]
    public async Task DeleteAsync_ShouldHandleExceptions()
    {
        // Arrange
        _collection = Substitute.For<IMongoCollection<StorageRegistryEntry>>();
        _collection.DeleteOneAsync(
                Arg.Any<FilterDefinition<StorageRegistryEntry>>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("Test exception"));

        _strategy = new MongoDBRegistryStrategy(
            _dateTimeProvider,
            Options.Create(_options),
            _collection,
            new NullLogger<MongoDBRegistryStrategy>());

        // Act
        var result = await _strategy.DeleteAsync("test-key", CancellationToken.None);

        // Assert
        result.Should().BeFailure()
            .And.HaveError("Failed to delete storage registry entry.")
            .Which.HasException<Exception>(e => e.Message == "Test exception").Should().BeTrue();
    }

    [Test]
    public async Task DeleteAsync_ShouldNotFailWhenEntryNotFound()
    {
        // Arrange

        // Act
        var result = await _strategy.DeleteAsync("test-key", CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var entry = await _collection.Find(x => x.Key == "test-key").SingleOrDefaultAsync();
        entry.Should().BeNull();
    }

    [Test]
    public async Task GetAsync_ShouldHandleExceptions()
    {
        // Arrange
        _collection = Substitute.For<IMongoCollection<StorageRegistryEntry>>();
        _collection.FindAsync(
                Arg.Any<FilterDefinition<StorageRegistryEntry>>(),
                Arg.Any<FindOptions<StorageRegistryEntry>>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("Test exception"));

        _strategy = new MongoDBRegistryStrategy(
            _dateTimeProvider,
            Options.Create(_options),
            _collection,
            new NullLogger<MongoDBRegistryStrategy>());

        // Act
        var result = await _strategy.GetAsync("test-key", CancellationToken.None);

        // Assert
        result.Should().BeFailure().And.HaveReason("Failed to get storage registry entry.")
            .Which.HasException<Exception>(x => x.Message == "Test exception").Should().BeTrue();
    }

    [Test]
    public async Task GetAsync_ShouldReturnNotFoundError_WhenKeyDoesNotExist()
    {
        // Arrange
        var key = "non-existent-key";

        // Act
        var result = await _strategy.GetAsync(key, CancellationToken.None);

        // Assert
        result.Should().BeFailure().And.HaveReason<NotFoundError>("Not found.");
    }

    [Test]
    public async Task GetAsync_ShouldReturnStream_WhenKeyExists()
    {
        // Arrange
        var key = "test-key";
        var content = "test-data"u8.ToArray();
        await InsertStorageEntry(key, content, "test-file.txt");

        var expectedEntry = new StorageRegistryEntry
        {
            Key = key,
            FileName = "test-file.txt",
            ContentType = "UNKNOWN",
            Size = content.Length,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0)
        };

        // Act
        var result = await _strategy.GetAsync(key, CancellationToken.None);

        // Assert
        result.Should().BeSuccess()
            .And.Subject.Value.Should().BeEquivalentTo(expectedEntry);
    }

    [Test]
    public async Task GetExpiredTempFiles_ShouldNotReturnPreservedFiles()
    {
        // Arrange
        var entry = new StorageRegistryEntry
        {
            Key = "expired-key",
            FileName = "expired-file.txt",
            ContentType = "UNKNOWN",
            Size = 100,
            CreatedAt = _refDate.AddSeconds(-_options.TempFileLifespan - 1),
            Preserve = true
        };

        await _collection.InsertOneAsync(entry);

        // Act
        var result = await _strategy.GetExpiredTempFiles(CancellationToken.None);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().BeEmpty();
    }

    [Test]
    public async Task GetExpiredTempFiles_ShouldReturnEmpty_WhenNoExpiredFiles()
    {
        // Arrange
        var nonExpiredEntry = new StorageRegistryEntry
        {
            Key = "non-expired-key",
            FileName = "non-expired-file.txt",
            ContentType = "UNKNOWN",
            Size = 100,
            CreatedAt = _refDate,
            Preserve = false
        };

        await _collection.InsertOneAsync(nonExpiredEntry);

        // Act
        var result = await _strategy.GetExpiredTempFiles(CancellationToken.None);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().BeEmpty();
    }

    [Test]
    public async Task GetExpiredTempFiles_ShouldReturnExpiredFiles()
    {
        // Arrange
        var expiredEntry = new StorageRegistryEntry
        {
            Key = "expired-key",
            FileName = "expired-file.txt",
            ContentType = "UNKNOWN",
            Size = 100,
            CreatedAt = _refDate.AddSeconds(-_options.TempFileLifespan),
            Preserve = false
        };

        var nonExpiredEntry = new StorageRegistryEntry
        {
            Key = "non-expired-key",
            FileName = "non-expired-file.txt",
            ContentType = "UNKNOWN",
            Size = 100,
            CreatedAt = _refDate,
            Preserve = false
        };

        await _collection.InsertOneAsync(expiredEntry);
        await _collection.InsertOneAsync(nonExpiredEntry);

        // Act
        var result = await _strategy.GetExpiredTempFiles(CancellationToken.None);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().ContainSingle()
            .Which.Should().BeEquivalentTo(expiredEntry);
    }

    [OneTimeSetUp]
    public async Task OneTimeSetUp() => await _mongoDbFixture.InitializeAsync();

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _mongoDbFixture.DisposeAsync();

    [Test]
    public async Task RegisterNewFileAsync_ShouldCreateNewEntry()
    {
        // Arrange
        var expectedEntry = new StorageRegistryEntry
        {
            Key = "test-key",
            FileName = "test-file.txt",
            ContentType = "UNKNOWN",
            Size = "test-data"u8.ToArray().Length,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0)
        };

        // Act
        var result = await InsertStorageEntry(
            "test-key",
            "test-data"u8.ToArray(),
            "test-file.txt");

        // Assert
        result.IsSuccess.Should().BeTrue();

        var entry = await _collection.Find(x => x.Key == "test-key").FirstOrDefaultAsync();
        entry.Should().NotBeNull().And.BeEquivalentTo(expectedEntry);
    }

    [Test]
    public async Task RegisterNewFileAsync_ShouldNotCreateDuplicateEntry()
    {
        // Arrange
        await InsertStorageEntry("test-key", "test-data"u8.ToArray(), "test-file.txt");
        var firstEntry = await _collection.Find(x => x.Key == "test-key").SingleOrDefaultAsync();

        // Act
        var result = await InsertStorageEntry("test-key", "test-data1"u8.ToArray(), "test-file2.txt");


        // Assert
        result.Should().BeFailure();

        var testEntry = await _collection.Find(x => x.Key == "test-key").SingleOrDefaultAsync();
        testEntry.Should().NotBeNull().And.BeEquivalentTo(firstEntry);
    }

    [Test]
    public async Task SetPreserveAsync_ShouldHandleExceptions()
    {
        // Arrange
        _collection = Substitute.For<IMongoCollection<StorageRegistryEntry>>();
        _collection.UpdateOneAsync(
                Arg.Any<FilterDefinition<StorageRegistryEntry>>(),
                Arg.Any<UpdateDefinition<StorageRegistryEntry>>(),
                Arg.Any<UpdateOptions>(),
                Arg.Any<CancellationToken>())
            .ThrowsAsync(new Exception("Test exception"));

        _strategy = new MongoDBRegistryStrategy(
            _dateTimeProvider,
            Options.Create(_options),
            _collection,
            new NullLogger<MongoDBRegistryStrategy>());

        // Act
        var result = await _strategy.SetPreserveAsync("test-key", true);

        // Assert
        result.Should().BeFailure().And.HaveReason("Failed to set preserve flag.")
            .Which.HasException<Exception>(x => x.Message == "Test exception").Should().BeTrue();
    }

    [Test]
    public async Task SetPreserveAsync_ShouldReturnNotFoundError_WhenInvalidKey()
    {
        // Arrange

        // Act
        var result = await _strategy.SetPreserveAsync("test-key", true);

        // Assert
        result.Should().BeFailure().And.HaveReason<NotFoundError>("Not found.");
    }

    [Test]
    public async Task SetPreserveAsync_ShouldSetPreserveFlag()
    {
        // Arrange
        await InsertStorageEntry("test-key", "test-data"u8.ToArray(), "test-file.txt");

        // Act
        var result = await _strategy.SetPreserveAsync("test-key", true);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var entry = await _collection.Find(x => x.Key == "test-key").SingleOrDefaultAsync();
        entry.Should().NotBeNull().And.BeEquivalentTo(new { Preserve = true });
    }

    [SetUp]
    public async Task SetUp()
    {
        SetupDateTimeProvider();

        SetupOptions();

        await SetupMongoCollection();

        _strategy = new MongoDBRegistryStrategy(
            _dateTimeProvider,
            Options.Create(_options),
            _collection,
            new NullLogger<MongoDBRegistryStrategy>());
    }

    [Test]
    public async Task ValidateKeyAsync_ShouldReturnFailure_WhenEntryDoesNotExist()
    {
        // Arrange

        // Act
        var result = await _strategy.ValidateKeyAsync("test-key", CancellationToken.None);

        // Assert
        result.Should().BeFailure().And.HaveReason("Key not found.");
    }

    [Test]
    public async Task ValidateKeyAsync_ShouldReturnSuccess_WhenEntryExists()
    {
        // Arrange
        await InsertStorageEntry("test-key", "test-data"u8.ToArray(), "test-file.txt");

        // Act
        var result = await _strategy.ValidateKeyAsync("test-key", CancellationToken.None);

        // Assert
        result.Should().BeSuccess();
    }

    private async Task<Result> InsertStorageEntry(string key, byte[] content, string name)
    {
        var file = new FormFile(
            new MemoryStream(content),
            0,
            content.Length,
            name,
            name);
        return await _strategy.RegisterNewFileAsync(key, file, CancellationToken.None);
    }

    private void SetupDateTimeProvider()
    {
        _refDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _dateTimeProvider.UtcNow().Returns(_refDate);
    }

    private Task SetupMongoCollection()
    {
        var mongoClient = new MongoClient(_mongoDbFixture.ConnectionString);
        var database = mongoClient.GetDatabase(Guid.NewGuid().ToString());
        _collection = database.GetCollection<StorageRegistryEntry>("test-collection");
        return Task.CompletedTask;
    }

    private void SetupOptions() =>
        _options = new RegistryStrategyOptions { TempFileLifespan = 1 };
}