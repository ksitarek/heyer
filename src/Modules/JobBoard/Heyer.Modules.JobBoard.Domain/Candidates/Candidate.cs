using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.Candidates;

public class Candidate : Entity
{
    public CandidateId Id { get; } = null!;

    private string _firstName = null!;
    private string _lastName = null!;
    private Email _email = null!;
    private ResumeKey _resumeKey = null!;
    private bool _includeInCandidatePool;
    private Dictionary<string, object> _attributes = null!;

    // For EF Core
    private Candidate()
    {
    }
    
    private Candidate(string firstName, string lastName, Email email, ResumeKey resumeKey, bool includeInCandidatePool, Dictionary<string, object> attributes)
    {
        Id = CandidateId.CreateNew();
        
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _resumeKey = resumeKey;
        _includeInCandidatePool = includeInCandidatePool;
        _attributes = attributes;

        AddDomainEvent(new CandidateCreated(Id));
    }
    
    public static Candidate Create(
        string firstName,
        string lastName, 
        Email email, 
        ResumeKey resumeKey,
        bool includeInCandidatePool, 
        Dictionary<string, object> attributes)
    {
        return new Candidate(firstName, lastName, email, resumeKey, includeInCandidatePool, attributes);
    }
}