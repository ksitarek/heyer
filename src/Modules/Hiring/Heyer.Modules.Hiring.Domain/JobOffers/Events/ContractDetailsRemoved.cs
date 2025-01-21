using Heyer.BuildingBlocks.Domain;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Domain.JobOffers.Events;

public record ContractDetailsRemoved(JobOfferId JobOfferId, EmploymentType EmploymentType) : DomainEvent;