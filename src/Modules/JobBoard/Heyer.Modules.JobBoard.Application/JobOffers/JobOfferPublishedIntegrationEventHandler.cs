using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;
using Heyer.Modules.JobBoard.Domain.JobOffers;

namespace Heyer.Modules.JobBoard.Application.JobOffers;

public class JobOfferPublishedIntegrationEventHandler : IEventHandler<JobOfferPublishedIntegrationEvent>
{
    private readonly IPublishedJobOffersRepository _publishedJobOffersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public JobOfferPublishedIntegrationEventHandler(IPublishedJobOffersRepository publishedJobOffersRepository,
                                                    IUnitOfWork unitOfWork)
    {
        _publishedJobOffersRepository = publishedJobOffersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(JobOfferPublishedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        var publishedJobOffer = PublishedJobOffer.CreateNew(
            new PublishedJobOfferId(notification.JobOfferId),
            notification.CompanyDetails,
            notification.OfferSummary,
            notification.JobDescription,
            notification.RemoteWork,
            notification.ContractsDetails,
            notification.Location,
            notification.PublishedUntil,
            notification.Requirements);

        await _publishedJobOffersRepository.AddAsync(publishedJobOffer, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}