using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.JobBoard.Domain.JobOffers.Events;

public record NewJobOfferApplicationCreated(
    PublishedJobOfferId PublishedJobOfferId,
    CompanyDetails CompanyDetails,
    JobOfferApplication JobOfferApplication)
    : DomainEvent;