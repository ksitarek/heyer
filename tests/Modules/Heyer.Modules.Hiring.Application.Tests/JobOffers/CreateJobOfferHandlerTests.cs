using FluentResults;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Tests.Extensions;
using Heyer.Modules.Hiring.Application.JobOffers.Create;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.PublishedLanguage.DTOs;
using NSubstitute;
using NSubstitute.Extensions;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class CreateJobOfferHandlerTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private CreateJobOfferHandler _handler;
    private IJobOffersRepository _jobOffersRepository;
    private CreateJobOffer _testRequest;
    private IUserDataProvider _userDataProvider;

    [Test]
    public async Task Handle_ShouldNotThrowWhenJobOffersRepositoryFails()
    {
        // Arrange
        _jobOffersRepository.Configure().AddAsync(Arg.Any<JobOffer>(), _cancellationToken)
            .Returns(Result.Fail("Error"));

        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);

        // Assert
        result.ShouldBeFailure("Error");
    }

    [Test]
    public async Task Handle_ShouldSucceed()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);

        // Assert
        result.ShouldBeSuccess();
        result.Value.ShouldNotBeEmpty();

        await _jobOffersRepository.Received(1)
            .AddAsync(Arg.Is<JobOffer>(jo => jo.Id.Guid == result.Value), _cancellationToken);
    }

    [SetUp]
    public void SetUp()
    {
        _userDataProvider = Substitute.For<IUserDataProvider>();
        _userDataProvider.Configure().CompanyId.Returns(Guid.NewGuid());
        _userDataProvider.Configure().CompanyName.Returns("ACME Inc.");

        _jobOffersRepository = Substitute.For<IJobOffersRepository>();
        _jobOffersRepository.Configure().AddAsync(Arg.Any<JobOffer>(), _cancellationToken)
            .Returns(Result.Ok());

        _handler = new CreateJobOfferHandler(_userDataProvider, _jobOffersRepository);

        _testRequest = new CreateJobOffer("Offer Summary", "Job Description", RemoteWork.Hybrid);
    }
}