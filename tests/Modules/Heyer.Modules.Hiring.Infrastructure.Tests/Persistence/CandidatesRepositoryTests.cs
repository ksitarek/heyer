using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Heyer.Modules.Hiring.Infrastructure.Tests.Persistence;

[Category("Integration")]
public class CandidatesRepositoryTests
{
    private HiringDbContext _dbContext = null!;
    private CandidatesRepository _repository = null!;

    [Test]
    public async Task AddCandidate_ShouldReturnOkResult()
    {
        // Arrange
        var candidate = TestCandidateBuilder.Create().Build();

        // Act
        var result = await _repository.AddCandidate(candidate);

        // Assert
        result.ShouldBeSuccess();
    }

    [Test]
    public async Task AddCandidate_WhenFails_ShouldReturnFailedResult()
    {
        // Arrange

        // Act
        var result = await _repository.AddCandidate(default!);

        // Assert
        result.ShouldBeFailure();
    }

    [Test]
    public async Task GetCandidateById_WhenCandidateExists_ShouldReturnCandidate()
    {
        // Arrange
        var candidate = TestCandidateBuilder.Create().Build();

        await _dbContext.Candidates.AddAsync(candidate);

        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetCandidateById(candidate.Id);

        // Assert
        result.ShouldBe(candidate);
    }

    [Test]
    public async Task GetCandidateById_WhenCandidateNotExists_ShouldReturnNull()
    {
        // Arrange

        // Act
        var result = await _repository.GetCandidateById(CandidateId.CreateNew());

        // Assert
        result.ShouldBeNull();
    }

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<HiringDbContext>()
            .UseNpgsql(PersistenceTestsFixture.ConnectionString.Replace("master", Guid.NewGuid().ToString()))
            .Options;

        _dbContext = new HiringDbContext(options);

        _dbContext.Database.EnsureCreated();

        _repository = new CandidatesRepository(_dbContext);
    }

    [TearDown]
    public async Task TearDown() => await _dbContext.DisposeAsync();
}