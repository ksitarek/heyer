using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.JobBoard.Domain.Candidates;

public class Candidate : Entity
{
    public CandidateId Id { get; }
    
    private string _firstName;
    private string _lastName;
    private Email _email;
    private ResumeKey _resumeKey;
    private bool _includeInCandidatePool;
    private Dictionary<string, object> _attributes;
    
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