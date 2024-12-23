using FluentAssertions;
using Heyer.API.Client.PublishedLanguage;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Application.Mapping;
using RemoteWork = Heyer.Modules.Hiring.Domain.JobOffers.RemoteWork;

namespace Heyer.Modules.Hiring.Application.Tests;

[Category("Unit")]
public class JobOfferMappingsTests
{
    [TestCase(API.Client.PublishedLanguage.RemoteWork.Unknown, RemoteWork.Unknown)]
    [TestCase(API.Client.PublishedLanguage.RemoteWork.No, RemoteWork.No)]
    [TestCase(API.Client.PublishedLanguage.RemoteWork.Hybrid, RemoteWork.Hybrid)]
    [TestCase(API.Client.PublishedLanguage.RemoteWork.Yes, RemoteWork.Yes)]
    public void MapToCommand_WithValidRequest_ShouldReturnCreateJobOfferCommand(
        API.Client.PublishedLanguage.RemoteWork inputRemoteWork,
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