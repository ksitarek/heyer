using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.Mapping;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using Shouldly;

namespace Heyer.Modules.Hiring.Application.Tests;

[Category("Unit")]
public class JobOfferMappingsTests
{
    [TestCase(RemoteWork.Unknown, RemoteWork.Unknown)]
    [TestCase(RemoteWork.No, RemoteWork.No)]
    [TestCase(RemoteWork.Hybrid, RemoteWork.Hybrid)]
    [TestCase(RemoteWork.Yes, RemoteWork.Yes)]
    public void MapToCommand_WithValidRequest_ShouldReturnCreateJobOfferCommand(
        RemoteWork inputRemoteWork,
        RemoteWork expectedRemoteWork)
    {
        // Arrange
        var request = new CreateJobOfferRequest(
            "OfferSummary",
            "JobDescription",
            inputRemoteWork);

        // Act
        var result = request.MapToCommand();

        // Assert
        result.ShouldBeOfType<CreateJobOffer>();
        result.OfferSummary.ShouldBe(request.OfferSummary);
        result.JobDescription.ShouldBe(request.JobDescription);
        result.RemoteWork.ShouldBe(expectedRemoteWork);
    }

    [Test]
    public void MapToJobOfferDetails_WithValidJobOffer_ShouldReturnJobOfferDetails()
    {
        // Arrange
        var jobOffer = TestJobOfferBuilder.Create()
            .WithRandomContractDetails()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        // Act
        var result = jobOffer.MapToJobOfferDetails();

        // Assert
        result.ShouldBeOfType<JobOfferDetails>();
        result.Id.ShouldBe(jobOffer.Id.Guid);
        result.OfferSummary.ShouldBe(jobOffer.OfferSummary);
        result.JobDescription.ShouldBe(jobOffer.JobDescription);
        result.PublishedAt.ShouldBe(jobOffer.PublishedAt);
        result.PublishedUntil.ShouldBe(jobOffer.PublishedUntil);
        result.OfficeLocation.ShouldBe(jobOffer.Location);
        result.RemoteWork.ShouldBe(jobOffer.RemoteWork);
        result.Requirements.ShouldBe(jobOffer.Requirements);
        result.ContractsDetails.ShouldBe(jobOffer.ContractsDetails);
    }
}