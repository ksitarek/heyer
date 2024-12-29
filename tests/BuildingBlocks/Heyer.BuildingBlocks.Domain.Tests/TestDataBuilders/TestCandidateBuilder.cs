using Bogus;
using Heyer.Modules.Hiring.Domain.Candidates;

namespace Heyer.BuildingBlocks.Domain.Tests.TestDataBuilders;

internal class TestCandidateBuilder
{
    internal static readonly Faker _f = new();
    private readonly Email _email;
    private readonly string _firstName;
    private readonly string _lastName;
    private readonly ResumeKey _resumeKey;

    private TestCandidateBuilder(string firstName, string lastName, Email email, ResumeKey resumeKey)
    {
        _firstName = firstName;
        _lastName = lastName;
        _email = email;
        _resumeKey = resumeKey;
    }

    private Randomizer R => _f.Random;

    public static TestCandidateBuilder Create() =>
        new(
            _f.Name.FirstName(),
            _f.Name.LastName(),
            new Email(_f.Internet.Email()),
            new ResumeKey(_f.Random.Guid().ToString()));

    public Candidate Build() =>
        Candidate.Create(
            _firstName,
            _lastName,
            _email,
            _resumeKey,
            R.Bool(),
            new Dictionary<string, object>());
}