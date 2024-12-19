using Bogus;
using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.Modules.JobBoard.Application.JobOffers.Create;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.Modules.JobBoard.Application.Tests.JobOffers;

[Category("Unit")]
public class CreateJobOfferHandlerTests
{
    private IUserDataProvider _userDataProvider;
    private IJobOffersRepository _jobOffersRepository;
    private CreateJobOfferHandler _handler;
    private CreateJobOffer _testRequest;

    private static CancellationToken _cancellationToken = CancellationToken.None;

    [SetUp]
    public void SetUp()
    {
        _userDataProvider = Substitute.For<IUserDataProvider>();
        _userDataProvider.Configure().CompanyId.Returns(Guid.NewGuid());
        _userDataProvider.Configure().CompanyName.Returns("ACME Inc.");
        
        _jobOffersRepository = Substitute.For<IJobOffersRepository>();
        _jobOffersRepository.Configure().AddAsync(Arg.Any<JobOffer>(), _cancellationToken).Returns(Result.Ok());

        _handler = new CreateJobOfferHandler(_userDataProvider, _jobOffersRepository);
        
        _testRequest = new CreateJobOffer("Offer Summary", "Job Description", RemoteWork.Hybrid);
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

        await _jobOffersRepository.Received(1).AddAsync(Arg.Is<JobOffer>(predicate: jo => jo.Id.Guid == result.Value), _cancellationToken);
    }

    [Test]
    public async Task Handle_ShouldNotThrowWhenJobOffersRepositoryFails()
    {
        // Arrange
        _jobOffersRepository.Configure().AddAsync(Arg.Any<JobOffer>(), _cancellationToken).Returns(Result.Fail("Error"));
        
        // Act
        var result = await _handler.Handle(_testRequest, _cancellationToken);
        
        // Assert
        result.Should().BeFailure();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Error");
    }
}