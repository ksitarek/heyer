using FluentAssertions;
using Heyer.Modules.Candidates.Domain.Candidates;

namespace Heyer.Modules.Candidates.Domain.Tests;

[Category("Unit")]
public class CandidateTests
{
    [Test]
    public void Candidate_WhenCreate_ShouldCreateInstance()
    {
        // Arrange
        
        // Act
        var candidate = Candidate.CreateNew("John", "Doe", "john.doe@example.com", "123456789", DateTime.UtcNow, new());

        // Assert
        candidate.Should().NotBeNull();
        candidate.Id.Should().NotBeNull();
        candidate.Id.Guid.Should().NotBeEmpty();
    }

    [Test]
    public void Candidate_WhenCreate_ShouldRaiseCandidateCreated()
    {
        // Arrange
        
        // Act
        var candidate = Candidate.CreateNew("John", "Doe", "john.doe@example.com", "123456789", DateTime.UtcNow, new());

        // Assert
        var domainEvent = candidate.DomainEvents.Should().HaveCount(1).And.Subject.SingleOrDefault();

        domainEvent.Should().NotBeNull().And.BeOfType<CandidateCreated>();
        domainEvent!.Id.Should().NotBeEmpty();
        domainEvent.OccurredOn.Should().BeWithin(TimeSpan.FromMilliseconds(1));
        ((CandidateCreated)domainEvent).CandidateId.Should().BeSameAs(candidate.Id);
    }
}