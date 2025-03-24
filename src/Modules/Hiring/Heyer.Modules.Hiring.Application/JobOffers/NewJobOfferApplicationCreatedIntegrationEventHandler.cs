using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;
using MediatR;

namespace Heyer.Modules.Hiring.Application.JobOffers;

public class
    NewJobOfferApplicationCreatedIntegrationEventHandler : IEventHandler<
    NewJobOfferApplicationCreatedIntegrationEventWithContext>
{
    private readonly IMediator _mediator;

    public NewJobOfferApplicationCreatedIntegrationEventHandler(IMediator mediator) => _mediator = mediator;

    public async Task Handle(NewJobOfferApplicationCreatedIntegrationEventWithContext notification,
                             CancellationToken cancellationToken)
    {
        var command = new NewCandidateApplyToJobOffer(
            new JobOfferId(notification.JobOfferId),
            notification.FirstName,
            notification.LastName,
            notification.Email,
            notification.ResumeKey,
            notification.IncludeInCandidatePool,
            notification.Attributes);

        try
        {
            var result = await _mediator.Send(command, cancellationToken);

            if (result.IsFailed)
            {
                throw new Exception("Failed to execute NewCandidateApplyToJobOffer command, message: " + result);
            }
        }
        catch (Exception e)
        {
            throw new Exception("Failed to apply candidate to job offer", e);
        }
    }
}