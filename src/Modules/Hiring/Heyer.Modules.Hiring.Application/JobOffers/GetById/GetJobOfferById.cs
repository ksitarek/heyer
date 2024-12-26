using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.GetById;

public record GetJobOfferById(Guid Guid) : IQuery<JobOfferDetails>;