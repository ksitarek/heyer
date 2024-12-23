using FluentAssertions;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.Mapping;
using Heyer.Modules.Hiring.PublishedLanguage;

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
}