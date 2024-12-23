using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage;

namespace Heyer.Modules.Hiring.Application.JobOffers.GetById;

public record GetJobOfferById(Guid Guid) : IQuery<JobOfferDetails>;