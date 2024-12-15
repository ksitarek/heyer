using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.Modules.Candidates.Domain.Candidates;
using Heyer.Storage.API.Client;
using Microsoft.Extensions.Logging;

namespace Heyer.Modules.Candidates.Application.Candidates.Create;

public class CandidateCreatedEventHandler : IEventHandler<CandidateCreated>
{
    private readonly ILogger<CandidateCreatedEventHandler> _logger;
    private readonly IStorageApiClient _storageApiClient;
    private readonly ICandidateRepository _candidateRepository;

    public CandidateCreatedEventHandler(
        ILogger<CandidateCreatedEventHandler> logger,
        IStorageApiClient storageApiClient,
        ICandidateRepository candidateRepository)
    {
        _logger = logger;
        _storageApiClient = storageApiClient;
        _candidateRepository = candidateRepository;
    }
    
    public async Task Handle(CandidateCreated notification, CancellationToken cancellationToken)
    {
        var candidate = await _candidateRepository.GetByIdAsync(notification.CandidateId, cancellationToken);

        if(candidate == null)
        {
            _logger.LogError("Candidate with id: {candidateId} was not found.", notification.CandidateId);
            return;
        }
                
        await _storageApiClient.Preserve(candidate!.ResumeKey);
    }
}