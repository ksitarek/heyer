using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Results;
using Heyer.Modules.JobBoard.Application.JobOffers.NewCandidateApply;
using Heyer.Modules.JobBoard.Domain.Candidates;
using Heyer.Modules.JobBoard.Domain.Companies;
using Heyer.Modules.JobBoard.Domain.JobOffers;
using NSubstitute;
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

    [SetUp]
    public void SetUp()
    {
        _jobOffer.ClearDomainEvents();
        
        _jobOffersRepository = Substitute.For<IJobOffersRepository>();
        _jobOffersRepository.Configure()
            .GetJobOfferById(_jobOffer.Id, _cancellationToken).Returns(_jobOffer);

        _candidatesRepository = Substitute.For<ICandidatesRepository>();
        _candidatesRepository.Configure()
            .AddCandidate(Arg.Any<Candidate>(), _cancellationToken).Returns(Result.Ok());

        _handler = new NewCandidateApplyToJobOfferHandler(_jobOffersRepository, _candidatesRepository);
        
        _request = new NewCandidateApplyToJobOffer(
            _jobOffer.Id,
            "John",
            "Doe",
            "John.Doe@example.com",
            "resumeKey#1",
            true,
            new Dictionary<string, object>());
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
    }
}