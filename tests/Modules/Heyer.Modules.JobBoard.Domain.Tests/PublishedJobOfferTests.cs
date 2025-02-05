using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Modules.JobBoard.Domain.JobOffers.Events;
using Shouldly;

namespace Heyer.Modules.JobBoard.Domain.Tests;

[Category("Unit")]
public class PublishedJobOfferTests
{
    [Test]
    public void JobOfferShouldCreate()
    {
        // Arrange

        // Act
        var jobOffer = TestPublishedJobOfferBuilder.Create(Guid.CreateVersion7())
            .BuildTestData();

        // Assert
        jobOffer.ShouldNotBeNull();
        jobOffer.Id.ShouldNotBeNull();
        jobOffer.DomainEvents.ShouldContainSingle(
            domainEvent => domainEvent.GetType() == typeof(JobOfferPublished)
                           && ((JobOfferPublished)domainEvent).PublishedJobOfferId == jobOffer.Id);
    }
}