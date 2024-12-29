using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace Heyer.Modules.Hiring.Infrastructure.Tests.Persistence;

[Category("Integration")]
public class CandidatesRepositoryTests
{
    private HiringDbContext _dbContext;
    private MongoClient _mongoClient;
    private CandidatesRepository _repository;

    [Test]
    public async Task AddCandidate_ShouldReturnOkResult()
    {
        // Arrange
        var candidate = TestCandidateBuilder.Create().Build();

        // Act
        var result = await _repository.AddCandidate(candidate);

        // Assert
        result.Should().BeSuccess();
    }

    [Test]
    public async Task AddCandidate_WhenFails_ShouldReturnFailedResult()
    {
        // Arrange

        // Act
        var result = await _repository.AddCandidate(default!);

        // Assert
        result.Should().BeFailure();
    }

    [Test]
    [Ignore("MongoDB")]
    public async Task GetCandidateById_WhenCandidateExists_ShouldReturnCandidate()
    {
        // Arrange
        var candidate = TestCandidateBuilder.Create().Build();

        await _dbContext.Candidates.AddAsync(candidate);

        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetCandidateById(candidate.Id);

        // Assert
        result.Should().Be(candidate);
    }

    [Test]
    [Ignore("MongoDB")]
    public async Task GetCandidateById_WhenCandidateNotExists_ShouldReturnNull()
    {
        // Arrange

        // Act
        var result = await _repository.GetCandidateById(CandidateId.CreateNew());

        // Assert
        result.Should().BeNull();
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var connectionString = PersistenceTestsFixture.ConnectionString;
        _mongoClient = new MongoClient(connectionString);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown() => _mongoClient.Dispose();

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseMongoDB(_mongoClient, Guid.NewGuid().ToString())
            .Options;

        _dbContext = new HiringDbContext(options);

        _repository = new CandidatesRepository(_dbContext);
    }

    [TearDown]
    public async Task TearDown() => await _dbContext.DisposeAsync();
}