using Heyer.BuildingBlocks.Infrastructure.Messaging;
using MediatR;

namespace Heyer.Modules.Candidates.Application.Candidates.Create;

public record CreateCandidate(string FirstName, string LastName, string Email, string ResumeKey, bool IncludeInCandidatePool, Dictionary<string, object> Attributes) : ICommand;