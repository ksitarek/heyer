using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.Candidates;

public class Candidate : Entity
{
    private Dictionary<string, object> _attributes = null!;
    private Email _email = null!;

    private string _firstName = null!;
    private bool _includeInCandidatePool;
    private string _lastName = null!;
    private ResumeKey _resumeKey = null!;

    // For EF Core
    private Candidate()
    {
    }

    private Candidate(string firstName,
                      string lastName,
                      Email email,
                      ResumeKey resumeKey,
                      bool includeInCandidatePool,
                      Dictionary<string, object> attributes)
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

    public CandidateId Id { get; } = null!;

    public static Candidate Create(string firstName,
                                   string lastName,
                                   Email email,
                                   ResumeKey resumeKey,
                                   bool includeInCandidatePool,
                                   Dictionary<string, object> attributes) =>
        new(firstName, lastName, email, resumeKey, includeInCandidatePool, attributes);
}