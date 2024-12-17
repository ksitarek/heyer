using System.Collections.ObjectModel;
using FluentResults;

namespace Heyer.BuildingBlocks.Domain;

public abstract class Entity
{
    private List<DomainEvent>? _domainEvents;

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly() ?? ReadOnlyCollection<DomainEvent>.Empty;

    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents ??= new();
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
    
    protected Result ChallengeBusinessRules(params IBusinessRule[] businessRules)
    {
        var validationResult = new Result();
        foreach (var businessRule in businessRules)
        {
            var result = businessRule.Challenge();
            if (result.IsFailed)
            {
                validationResult.Reasons.AddRange(result.Reasons);
            }
        }

        return validationResult;
    }
}