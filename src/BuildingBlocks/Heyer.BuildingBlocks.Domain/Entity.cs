using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using FluentResults;
using Heyer.BuildingBlocks.Application.Results;

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
        var errors = new List<Error>();
        foreach (var businessRule in businessRules)
        {
            var result = businessRule.Challenge();
            if (result.IsFailed)
            {
                errors.Add(new Error(result.ToString()));
            }
        }

        if (!errors.Any())
        {
            return Result.Ok();
        }

        var businessRuleViolation = new BusinessRuleViolation();

        businessRuleViolation.Reasons.AddRange(errors);

        return businessRuleViolation;
    }

    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents ??= new List<DomainEvent>();
        _domainEvents.Add(@event);
    }
}