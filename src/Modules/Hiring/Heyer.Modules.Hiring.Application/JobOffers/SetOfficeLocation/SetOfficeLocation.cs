using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

namespace Heyer.Modules.Hiring.Application.JobOffers.SetOfficeLocation;

public record SetOfficeLocation(JobOfferId Id, OfficeLocation Location) : ICommand;