using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.Hiring.Application.Candidates.NewCandidateApply;
using Heyer.Modules.Hiring.Domain.Candidates;
using Heyer.Modules.Hiring.Domain.Companies;
using Heyer.Modules.Hiring.Domain.JobOffers;
using Heyer.Modules.Hiring.Domain.JobOffers.Events;
using Heyer.Modules.Hiring.PublishedLanguage;
using Heyer.Storage.API.Client;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.Modules.Hiring.Application.Tests.JobOffers;

[Category("Unit")]
public class NewCandidateApplyToJobOfferHandlerTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private readonly JobOffer _publishedJobOffer = JobOffer.CreateNew(
        new CompanyDetails(Guid.NewGuid(), "ACME"),
        "Summary",
        "Description",
        RemoteWork.Hybrid);

    private ICandidatesRepository _candidatesRepository;
    private NewCandidateApplyToJobOfferHandler _handler;
    private IJobOffersRepository _iPublishedJobOffersRepository;
    private NewCandidateApplyToJobOffer _request;
    private IStorageApiClient _storageApiClient;

    [Test]
    public async Task Handle_ShouldReturnErrorWhenAddCandidateFails()
    {
        // Arrange
        _candidatesRepository.Configure()
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken).Returns(Result.Fail("Error"));

        // Act
        var result = await _handler.Handle(_request, _cancellationToken);

        // Assert
        result.Should().BeFailure()
            .And.HaveError("Error");

        await _iPublishedJobOffersRepository.Received(1)
            .GetJobOfferById(_publishedJobOffer.Id, _cancellationToken);

        await _storageApiClient.Received(1)
            .Preserve(_request.ResumeKey);
    }

    [Test]
    public async Task Handle_ShouldReturnErrorWhenStorageApiClientFails()
    {
        // Arrange
        _storageApiClient.Configure()
            .Preserve(Arg.Any<string>()).ThrowsAsync(new Exception("Error"));

        // Act
        var result = await _handler.Handle(_request, _cancellationToken);

        // Assert
        result.Should().BeFailure().And.HaveError("Failed to preserve resume")
            .And.Subject.HasException<Exception>(x => x.Message == "Error").Should().BeTrue();


        await _iPublishedJobOffersRepository.Received(1)
            .GetJobOfferById(_publishedJobOffer.Id, _cancellationToken);

        await _candidatesRepository.Received(0)
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken);
    }

    [Test]
    public async Task Handle_ShouldReturnNotFoundWhenJobOfferNotFound()
    {
        // Arrange
        _iPublishedJobOffersRepository.Configure()
            .GetJobOfferById(_publishedJobOffer.Id, _cancellationToken).Returns(default(JobOffer));

        // Act
        var result = await _handler.Handle(_request, _cancellationToken);

        // Assert
        result.Should().BeFailure()
            .And.HaveError("Not found.").Which.HasError<NotFoundError>().Should().BeTrue();

        await _storageApiClient.Received(0)
            .Preserve(Arg.Any<string>());

        await _candidatesRepository.Received(0)
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken);
    }

    [Test]
    public async Task Handle_ShouldSucceed()
    {
        // Arrange

        // Act
        var result = await _handler.Handle(_request, _cancellationToken);

        // Assert
        result.Should().BeSuccess();
        var candidateApplied = _publishedJobOffer.DomainEvents.First() as CandidateApplied;

        candidateApplied.Should().NotBeNull();
        candidateApplied!.JobOfferId.Should().Be(_publishedJobOffer.Id);

        await _iPublishedJobOffersRepository.Received(1)
            .GetJobOfferById(_publishedJobOffer.Id, _cancellationToken);

        await _candidatesRepository.Received(1)
            .AddCandidate(Arg.Is<Candidate>(x => x.Id == candidateApplied.CandidateId), _cancellationToken);

        await _storageApiClient.Received(1)
            .Preserve(_request.ResumeKey);
    }

    [SetUp]
    public void SetUp()
    {
        MockJobOffersRepository();

        MockCandidatesRepository();

        MockStorageApiClient();

        _handler = new NewCandidateApplyToJobOfferHandler(_iPublishedJobOffersRepository,
                                                          _candidatesRepository,
                                                          _storageApiClient);

        _request = new NewCandidateApplyToJobOffer(
            _publishedJobOffer.Id,
            "John",
            "Doe",
            "John.Doe@example.com",
            "resumeKey#1",
            true,
            new Dictionary<string, object>());

        _publishedJobOffer.ClearDomainEvents();
    }

    private void MockCandidatesRepository()
    {
        _candidatesRepository = Substitute.For<ICandidatesRepository>();
        _candidatesRepository.Configure()
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken).Returns(Result.Ok());
    }

    private void MockJobOffersRepository()
    {
        _iPublishedJobOffersRepository = Substitute.For<IJobOffersRepository>();
        _iPublishedJobOffersRepository.Configure()
            .GetJobOfferById(_publishedJobOffer.Id, _cancellationToken).Returns(_publishedJobOffer);
    }

    private void MockStorageApiClient()
    {
        _storageApiClient = Substitute.For<IStorageApiClient>();
        _storageApiClient.Configure()
            .Preserve(Arg.Any<string>()).Returns(Task.CompletedTask);
    }
}