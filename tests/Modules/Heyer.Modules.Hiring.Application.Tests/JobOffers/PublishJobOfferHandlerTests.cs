using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Modules.Hiring.Application.JobOffers.Publish;
using Heyer.Modules.Hiring.Domain.JobOffers;
using NSubstitute;
using NSubstitute.Extensions;
using Shouldly;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class PublishJobOfferHandlerTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private PublishJobOfferHandler _handler = null!;
    private IJobOffersRepository _jobOffersRepository = null!;
    private DateTimeOffset _publishedUntil;
    private JobOffer _testJobOffer = null!;
    private PublishJobOffer _testRequest = null!;

    [Test]
    public async Task Handle_ShouldReturnNotFoundErrorWhenJobOfferIsNotFound()
    {
        // Arrange
        _jobOffersRepository.Configure().GetJobOfferById(Arg.Any<JobOfferId>(), _cancellationToken)
            .Returns(default(JobOffer));

        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);

        // Assert
        result.ShouldBeFailure();
        result.ShouldHaveError<NotFoundError>();
    }

    [Test]
    public async Task Handle_ShouldReturnOk()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);

        // Assert
        result.ShouldBeSuccess();

        _testJobOffer.PublishedUntil.ShouldBe(_publishedUntil);

        await _jobOffersRepository.Received(1).GetJobOfferById(_testJobOffer.Id, _cancellationToken);
    }

    [SetUp]
    public void SetUp()
    {
        _testJobOffer = TestJobOfferBuilder.Create()
            .WithRandomContractDetails()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        _publishedUntil = DateTimeOffset.UtcNow.AddDays(7);

        _jobOffersRepository = Substitute.For<IJobOffersRepository>();
        _jobOffersRepository.Configure().GetJobOfferById(Arg.Any<JobOfferId>(), _cancellationToken)
            .Returns(_testJobOffer);

        _handler = new PublishJobOfferHandler(_jobOffersRepository);

        _testRequest = new PublishJobOffer(_testJobOffer.Id.Guid, _publishedUntil);
    }
}