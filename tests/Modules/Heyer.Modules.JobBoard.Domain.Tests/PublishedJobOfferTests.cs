using FluentAssertions;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.Modules.JobBoard.Domain.JobOffers.Events;

namespace Heyer.Modules.JobBoard.Domain.Tests;

[Category("Unit")]
public class PublishedJobOfferTests
{
    [Test]
    public void JobOfferShouldCreate()
    {
        // Arrange

        // Act
        var jobOffer = TestPublishedJobOfferBuilder.Create(Guid.NewGuid())
            .BuildTestData();

        // Assert
        jobOffer.Should().NotBeNull();
        jobOffer.Id.Should().NotBeNull();
        jobOffer.DomainEvents.Should().ContainSingle(
            domainEvent => domainEvent.GetType() == typeof(JobOfferPublished)
                           && ((JobOfferPublished)domainEvent).PublishedJobOfferId == jobOffer.Id);
    }
}