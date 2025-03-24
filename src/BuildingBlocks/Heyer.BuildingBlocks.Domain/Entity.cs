using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using FluentResults;

namespace Heyer.BuildingBlocks.Domain;

public abstract class Entity
{
    [NotMapped] private List<DomainEvent>? _domainEvents;

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents =>
        _domainEvents?.AsReadOnly() ?? ReadOnlyCollection<DomainEvent>.Empty;

    public void ClearDomainEvents() => _domainEvents?.Clear();

    protected static Result ChallengeBusinessRules(params IBusinessRule[] businessRules)
    {
        var businessRuleViolationError = new BusinessRuleViolationError();
        foreach (var businessRule in businessRules)
        {
            var result = businessRule.Challenge();
            if (result.IsFailed)
            {
                businessRuleViolationError.Reasons.Add(result.Errors.OfType<Error>().First());
            }
        }

        return businessRuleViolationError.Reasons.Any()
            ? Result.Fail(businessRuleViolationError)
            : Result.Ok();
    }

    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(@event);
    }
}