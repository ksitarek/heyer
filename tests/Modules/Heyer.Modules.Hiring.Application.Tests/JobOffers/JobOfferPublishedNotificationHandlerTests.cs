using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Modules.Hiring.Application.JobOffers.Publish;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;
using NSubstitute;
using NSubstitute.Extensions;
using Shouldly;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class JobOfferPublishedNotificationHandlerTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private EventBusStub _eventBus = null!;
    private ExecutionContext _executionContext = null!;
    private JobOfferPublishedNotificationHandler _handler = null!;
    private IJobOffersRepository _jobOfferRepository = null!;
    private JobOfferPublishedNotification _notification = null!;
    private DateTimeOffset _publishedUntil;
    private JobOffer _testJobOffer = null!;

    [Test]
    public async Task Handle_ShouldPublishJobOfferPublishedIntegrationEvent_OnEventBus()
    {
        // Arrange

        // Act
        await _handler.Handle(_notification, _cancellationToken);

        // Assert
        var expectedIntegrationEvent = new JobOfferPublishedIntegrationEvent(
            Guid.CreateVersion7(),
            _notification.DomainEvent.OccurredOn,
            _testJobOffer.Id.Guid,
            new CompanyDetails(_executionContext.CompanyId, _executionContext.CompanyName),
            _testJobOffer.OfferSummary,
            _testJobOffer.JobDescription,
            _testJobOffer.RemoteWork,
            _testJobOffer.ContractsDetails!,
            _testJobOffer.Location!,
            _publishedUntil,
            _testJobOffer.Requirements!);

        _eventBus.IntegrationEvents.ShouldContainSingle();

        var integrationEvent = _eventBus.IntegrationEvents.OfType<JobOfferPublishedIntegrationEvent>().First();
        integrationEvent.OccurredOn.ShouldBe(expectedIntegrationEvent.OccurredOn);
        integrationEvent.JobOfferId.ShouldBe(expectedIntegrationEvent.JobOfferId);
        integrationEvent.CompanyDetails.ShouldBe(expectedIntegrationEvent.CompanyDetails);
        integrationEvent.OfferSummary.ShouldBe(expectedIntegrationEvent.OfferSummary);
        integrationEvent.JobDescription.ShouldBe(expectedIntegrationEvent.JobDescription);
        integrationEvent.RemoteWork.ShouldBe(expectedIntegrationEvent.RemoteWork);
        integrationEvent.ContractsDetails.ShouldBe(expectedIntegrationEvent.ContractsDetails);
        integrationEvent.Location.ShouldBe(expectedIntegrationEvent.Location);
        integrationEvent.PublishedUntil.ShouldBe(expectedIntegrationEvent.PublishedUntil);
        integrationEvent.Requirements.ShouldBe(expectedIntegrationEvent.Requirements);
    }

    [Test]
    public async Task Handle_ShouldThrowWhenJobOfferIsNotFound()
    {
        // Arrange
        _jobOfferRepository.Configure().GetJobOfferById(_testJobOffer.Id, _cancellationToken)
            .Returns(default(JobOffer));

        // Act
        var act = async () => await _handler.Handle(_notification, _cancellationToken);

        // Assert
        var exception = await act.ShouldThrowAsync<InvalidOperationException>();
        exception.Message.ShouldBe($"Job offer with id {_testJobOffer.Id} not found");

        _eventBus.IntegrationEvents.ShouldBeEmpty();
    }

    [SetUp]
    public void SetUp()
    {
        _publishedUntil = DateTimeOffset.UtcNow.AddDays(7);
        _executionContext = new ExecutionContext(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            Guid.CreateVersion7().ToString());

        PrepareJobOffer();

        _notification = new JobOfferPublishedNotification(
            _testJobOffer.Id.Guid,
            (_testJobOffer.DomainEvents.First() as JobOfferPublished)!,
            _executionContext);

        _eventBus = new EventBusStub();

        _jobOfferRepository = Substitute.For<IJobOffersRepository>();

        _jobOfferRepository.Configure().GetJobOfferById(_testJobOffer.Id, _cancellationToken).Returns(_testJobOffer);

        _handler = new JobOfferPublishedNotificationHandler(_eventBus, _jobOfferRepository);
    }

    private void PrepareJobOffer()
    {
        _testJobOffer = TestJobOfferBuilder.Create()
            .WithRandomContractDetails()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        _testJobOffer.ClearDomainEvents();

        _testJobOffer.Publish(_publishedUntil);
    }

    private class EventBusStub : IEventBus
    {
        public readonly List<IntegrationEvent> IntegrationEvents = new();

        public Task Publish<T>(T @event) where T : IntegrationEvent
        {
            IntegrationEvents.Add(@event);
            return Task.CompletedTask;
        }

        public Task Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent =>
            throw new NotImplementedException();
    }
}