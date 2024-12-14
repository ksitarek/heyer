using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Candidates.Domain.Candidates;

public class Candidate : Entity
{
    public CandidateId Id { get; set; }
    private readonly string _firstName;
    private readonly string _lastName;
    private readonly string _email;    
    private readonly string _resumeKey;
    private readonly DateTime _registeredAt;
    private readonly Dictionary<string, object> _attributes;
    
    private Candidate(
        string firstName,
        string lastName, 
        string email, 
        string resumeKey, 
        DateTime registeredAt, 
        Dictionary<string, object> attributes)
    {
        Id = CandidateId.CreateNew();
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _resumeKey = resumeKey;
        _registeredAt = registeredAt;
        _attributes = attributes;

        this.AddDomainEvent(new CandidateCreated(Id));
    }
    
    internal static Candidate CreateNew(
        string firstName,
        string lastName,
        string email,
        string resumeKey,
        DateTime registeredAt,
        Dictionary<string, object> attributes)
    {
        return new Candidate(firstName, lastName, email, resumeKey, registeredAt, attributes);
    }
}