using Heyer.BuildingBlocks.Infrastructure.Integration;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;

namespace Heyer.Modules.Hiring.Application.JobOffers.Publish;

public class
    JobOfferPublishedNotificationHandler : IDomainNotificationHandler<JobOfferPublishedNotification, JobOfferPublished>
{
    private readonly IEventBus _eventBus;
    private readonly IJobOffersRepository _jobOfferRepository;

    public JobOfferPublishedNotificationHandler(IEventBus eventBus,
                                                IJobOffersRepository jobOfferRepository)
    {
        _eventBus = eventBus;
        _jobOfferRepository = jobOfferRepository;
    }

    public async Task Handle(JobOfferPublishedNotification notification,
                             CancellationToken cancellationToken)
    {
        var jobOfferPublished = notification.DomainEvent;

        var companyDetails = new CompanyDetails(
            notification.ExecutionContext.CompanyId,
            notification.ExecutionContext.CompanyName);

        var jobOffer = await _jobOfferRepository.GetJobOfferById(jobOfferPublished.JobOfferId, cancellationToken);

        if (jobOffer == null)
        {
            // We should never step into this handler if the job offer does not exist
            throw new InvalidOperationException($"Job offer with id {jobOfferPublished.JobOfferId} not found");
        }

        var integrationEvent = new JobOfferPublishedIntegrationEvent(
            Guid.CreateVersion7(),
            jobOfferPublished.OccurredOn,
            jobOfferPublished.JobOfferId.Guid,
            companyDetails,
            jobOffer.OfferSummary,
            jobOffer.JobDescription,
            jobOffer.RemoteWork,
            jobOffer.ContractsDetails!,
            jobOffer.Location!,
            jobOffer.PublishedUntil,
            jobOffer.Requirements!);

        await _eventBus.Publish(integrationEvent);
    }
}