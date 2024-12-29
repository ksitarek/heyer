using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;
using Heyer.Modules.Hiring.Application.JobOffers.GetById;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using NSubstitute;
using NSubstitute.Extensions;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class GetJobOfferByIdHandlerTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private GetJobOfferByIdHandler _handler;
    private IJobOffersRepository _jobOffersRepository;
    private JobOffer _testJobOffer;

    [Test]
    public async Task Handle_ShouldReturnJobOfferDetails()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(new GetJobOfferById(_testJobOffer.Id.Guid), _cancellationToken);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().NotBeNull().And.BeOfType<JobOfferDetails>();
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
        result.Should().BeFailure();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<NotFoundError>();
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