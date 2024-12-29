using FluentAssertions;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.Mapping;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;

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
        result.Should().BeOfType<CreateJobOffer>();
        result.OfferSummary.Should().Be(request.OfferSummary);
        result.JobDescription.Should().Be(request.JobDescription);
        result.RemoteWork.Should().Be(expectedRemoteWork);
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
        result.Should().BeOfType<JobOfferDetails>();
        result.Id.Should().Be(jobOffer.Id.Guid);
        result.OfferSummary.Should().Be(jobOffer.OfferSummary);
        result.JobDescription.Should().Be(jobOffer.JobDescription);
        result.PublishedAt.Should().Be(jobOffer.PublishedAt);
        result.PublishedUntil.Should().Be(jobOffer.PublishedUntil);
        result.OfficeLocation.Should().Be(jobOffer.Location);
        result.RemoteWork.Should().Be(jobOffer.RemoteWork);
        result.Requirements.Should().BeEquivalentTo(jobOffer.Requirements);
        result.ContractsDetails.Should().BeEquivalentTo(jobOffer.ContractsDetails);
    }
}