using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using NSubstitute;
using NSubstitute.Extensions;

namespace Heyer.Modules.JobBoard.Application.Tests.JobOffers;

[Category("Unit")]
public class CreateJobOfferHandlerTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private CreateJobOfferHandler _handler;
    private IPublishedJobOffersRepository _iPublishedJobOffersRepository;
    private CreateJobOffer _testRequest;
    private IUserDataProvider _userDataProvider;

    [Test]
    public async Task Handle_ShouldNotThrowWhenJobOffersRepositoryFails()
    {
        // Arrange
        _iPublishedJobOffersRepository.Configure().AddAsync(Arg.Any<PublishedJobOffer>(), _cancellationToken)
            .Returns(Result.Fail("Error"));

        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);

        // Assert
        result.Should().BeFailure();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Error");
    }

    [Test]
    public async Task Handle_ShouldSucceed()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);

        // Assert
        result.Should().BeSuccess();
        result.Value.Should().NotBeEmpty();

        await _iPublishedJobOffersRepository.Received(1)
            .AddAsync(Arg.Is<PublishedJobOffer>(jo => jo.Id.Guid == result.Value), _cancellationToken);
    }

    [SetUp]
    public void SetUp()
    {
        _userDataProvider = Substitute.For<IUserDataProvider>();
        _userDataProvider.Configure().CompanyId.Returns(Guid.NewGuid());
        _userDataProvider.Configure().CompanyName.Returns("ACME Inc.");

        _iPublishedJobOffersRepository = Substitute.For<IPublishedJobOffersRepository>();
        _iPublishedJobOffersRepository.Configure().AddAsync(Arg.Any<PublishedJobOffer>(), _cancellationToken)
            .Returns(Result.Ok());

        _handler = new CreateJobOfferHandler(_userDataProvider, _iPublishedJobOffersRepository);

        _testRequest = new CreateJobOffer("Offer Summary", "Job Description", RemoteWork.Hybrid);
    }
}