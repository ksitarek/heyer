using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using Heyer.Storage.API.Client;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.Modules.JobBoard.Application.Tests.JobOffers;

[Category("Unit")]
public class NewCandidateApplyToJobOfferHandlerTests
{
    private IJobOffersRepository _jobOffersRepository;
    private ICandidatesRepository _candidatesRepository;
    private NewCandidateApplyToJobOfferHandler _handler;

    private readonly JobOffer _jobOffer = JobOffer.CreateNew(new CompanyDetails(CompanyId.CreateNew(), "ACME"),
                                                             "Summary", "Description", RemoteWork.Hybrid);

    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private NewCandidateApplyToJobOffer _request;
    private IStorageApiClient _storageApiClient;

    [SetUp]
    public void SetUp()
    {
        MockJobOffersRepository();

        MockCandidatesRepository();

        MockStorageApiClient();

        _handler = new NewCandidateApplyToJobOfferHandler(_jobOffersRepository, _candidatesRepository, _storageApiClient);
        
        _request = new NewCandidateApplyToJobOffer(
            _jobOffer.Id,
            "John",
            "Doe",
            "John.Doe@example.com",
            "resumeKey#1",
            true,
            new Dictionary<string, object>());
        
        _jobOffer.ClearDomainEvents();
    }

    [Test]
    public async Task Handle_ShouldSucceed()
    {
        // Arrange
        
        // Act
        var result = await _handler.Handle(_request, _cancellationToken);
        
        // Assert
        result.Should().BeSuccess();
        var candidateApplied = _jobOffer.DomainEvents.First() as CandidateApplied;

        candidateApplied.Should().NotBeNull();
        candidateApplied!.JobOfferId.Should().Be(_jobOffer.Id);
        
        await _jobOffersRepository.Received(1)
            .GetJobOfferById(_jobOffer.Id, _cancellationToken);
        
        await _candidatesRepository.Received(1)
            .AddCandidate(Arg.Is<Candidate>(x => x.Id == candidateApplied!.CandidateId), _cancellationToken);

        await _storageApiClient.Received(1)
            .Preserve(_request.ResumeKey);
    }

    [Test]
    public async Task Handle_ShouldReturnNotFoundWhenJobOfferNotFound()
    {
        // Arrange
        _jobOffersRepository.Configure()
            .GetJobOfferById(_jobOffer.Id, _cancellationToken).Returns(default(JobOffer));
        
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
        
        await _jobOffersRepository.Received(1)
            .GetJobOfferById(_jobOffer.Id, _cancellationToken);

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
        
        
        await _jobOffersRepository.Received(1)
            .GetJobOfferById(_jobOffer.Id, _cancellationToken);

        await _candidatesRepository.Received(0)
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken);
    }

    private void MockJobOffersRepository()
    {
        _jobOffersRepository = Substitute.For<IJobOffersRepository>();
        _jobOffersRepository.Configure()
            .GetJobOfferById(_jobOffer.Id, _cancellationToken).Returns(_jobOffer);
    }

    private void MockCandidatesRepository()
    {
        _candidatesRepository = Substitute.For<ICandidatesRepository>();
        _candidatesRepository.Configure()
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken).Returns(Result.Ok());
    }

    private void MockStorageApiClient()
    {
        _storageApiClient = Substitute.For<IStorageApiClient>();
        _storageApiClient.Configure()
            .Preserve(Arg.Any<string>()).Returns(Task.CompletedTask);
    }
}