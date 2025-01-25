using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Modules.Hiring.Application.JobOffers.GetById;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using NSubstitute;
using NSubstitute.Extensions;
using Shouldly;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class GetJobOfferByIdHandlerTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private GetJobOfferByIdHandler _handler = null!;
    private IJobOffersRepository _jobOffersRepository = null!;
    private JobOffer _testJobOffer = null!;

    [Test]
    public async Task Handle_ShouldReturnJobOfferDetails()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(new GetJobOfferById(_testJobOffer.Id.Guid), _cancellationToken);

        // Assert
        result.ShouldBeSuccess();
        result.Value.ShouldNotBeNull();
        result.Value.ShouldBeOfType<JobOfferDetails>();
    }

    [Test]
    public async Task Handle_ShouldReturnNotFoundErrorWhenJobOfferIsNotFound()
    {
        // Arrange
        _jobOffersRepository.Configure().GetJobOfferById(_testJobOffer.Id, _cancellationToken)
            .Returns(default(JobOffer));

        // Act
        var result = await _handler.Handle(new GetJobOfferById(_testJobOffer.Id.Guid), _cancellationToken);

        // Assert
        result.ShouldBeFailure();
        result.ShouldHaveError<NotFoundError>();
    }

    [SetUp]
    public void SetUp()
    {
        _testJobOffer = TestJobOfferBuilder.Create()
            .WithRandomContractDetails()
            .WithRandomOfficeLocation()
            .WithRandomRequirements()
            .Build();

        _jobOffersRepository = Substitute.For<IJobOffersRepository>();

        _jobOffersRepository.Configure().GetJobOfferById(_testJobOffer.Id, _cancellationToken)
            .Returns(_testJobOffer);

        _handler = new GetJobOfferByIdHandler(_jobOffersRepository);
    }
}