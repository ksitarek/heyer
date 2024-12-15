using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Candidates.Domain.Candidates;

public class Candidate : Entity
{
    public CandidateId Id { get; set; }
    public string ResumeKey => _resumeKey;

    private readonly string _firstName;
    private readonly string _lastName;
    private readonly string _email;
    private readonly string _resumeKey;
    private readonly bool _includeInCandidatePool;
    private readonly DateTime _registeredAt;
    private readonly Dictionary<string, object> _attributes;

    private Candidate(
        string firstName,
        string lastName, 
        string email, 
        string resumeKey, 
        bool includeInCandidatePool,
        DateTime registeredAt, 
        Dictionary<string, object> attributes)
    {
        Id = CandidateId.CreateNew();
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _resumeKey = resumeKey;
        _includeInCandidatePool = includeInCandidatePool;
        _registeredAt = registeredAt;
        _attributes = attributes;

        this.AddDomainEvent(new CandidateCreated(Id));
    }
    
    public static Candidate CreateNew(
        string firstName,
        string lastName,
        string email,
        string resumeKey,
        bool includeInCandidatePool,
        DateTime registeredAt,
        Dictionary<string, object> attributes)
    {
        return new Candidate(
            firstName, 
            lastName, 
            email, 
            resumeKey, 
            includeInCandidatePool, 
            registeredAt, 
            attributes);
    }
}