using Heyer.BuildingBlocks.Domain;

namespace Heyer.Modules.Hiring.Domain.Candidates;

public class Candidate : Entity
{
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

        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ResumeKey = resumeKey;
        IncludeInCandidatePool = includeInCandidatePool;
        // Attributes = attributes;

        AddDomainEvent(new CandidateCreated(Id));
    }

    public CandidateId Id { get; } = null!;

    // public Dictionary<string, object> Attributes { get; private set; } = null!;
    public Email Email { get; private set; } = null!;

    public string FirstName { get; private set; } = null!;
    public bool IncludeInCandidatePool { get; private set; }

    public string LastName { get; private set; } = null!;
    public ResumeKey ResumeKey { get; private set; } = null!;

    public static Candidate Create(string firstName,
                                   string lastName,
                                   Email email,
                                   ResumeKey resumeKey,
                                   bool includeInCandidatePool,
                                   Dictionary<string, object> attributes)
    {
        if (attributes == null)
        {
            throw new ArgumentNullException(nameof(attributes));
        }

        return new Candidate(firstName, lastName, email, resumeKey, includeInCandidatePool, attributes);
    }
}