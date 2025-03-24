using System.Text.Json.Serialization;
using Heyer.BuildingBlocks.Infrastructure.Integration;
using ExecutionContext = Heyer.BuildingBlocks.Application.Authorization.ExecutionContext;

namespace Heyer.Modules.Hiring.PublishedLanguage.IntegrationEvents;

public record NewJobOfferApplicationCreatedIntegrationEventWithContext : IntegrationEventWithContext
{
    [JsonConstructor]
    public NewJobOfferApplicationCreatedIntegrationEventWithContext(Guid id,
                                                                    DateTime occurredOn,
                                                                    ExecutionContext executionContext,
                                                                    Guid jobOfferId,
                                                                    string firstName,
                                                                    string lastName,
                                                                    string email,
                                                                    string resumeKey,
                                                                    bool includeInCandidatePool,
                                                                    Dictionary<string, object>? attributes) : base(
        id,
        occurredOn,
        executionContext)
    {
        JobOfferId = jobOfferId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ResumeKey = resumeKey;
        IncludeInCandidatePool = includeInCandidatePool;
        Attributes = attributes ?? new Dictionary<string, object>();
    }

    public Guid JobOfferId { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string ResumeKey { get; set; }
    public bool IncludeInCandidatePool { get; set; }
    public Dictionary<string, object> Attributes { get; set; }
}