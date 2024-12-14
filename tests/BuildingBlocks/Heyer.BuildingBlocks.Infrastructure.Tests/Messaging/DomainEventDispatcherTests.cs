using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Domain;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.BuildingBlocks.Infrastructure.Tests.Messaging;

[Category("Unit")]
public class DomainEventDispatcherTests
{
    private IMediator _mediator;
    private IDomainEventsAccessor _domainEventsAccessor;
    private CancellationToken _cancellationToken = CancellationToken.None;
    
    private List<DomainEvent> _domainEvents = new()
    {
        new FakeDomainEvent(Guid.NewGuid()),
        new FakeDomainEvent(Guid.NewGuid())
    };

    private DomainEventDispatcher _domainEventDispatcher;

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();
        _mediator.Configure().Publish(
                Arg.Any<FakeDomainEvent>(),
                _cancellationToken
            ).Returns(Task.CompletedTask);
        
        _domainEventsAccessor = Substitute.For<IDomainEventsAccessor>();
        _domainEventsAccessor.Configure().GetAllDomainEvents().Returns(_domainEvents);
        
        _domainEventDispatcher = new DomainEventDispatcher(_mediator, _domainEventsAccessor);
    }
    
    [Test]
    public async Task DispatchEventsAsync_WhenCalled_ShouldPublishAllDomainEvents()
    {
        // Act
        var result = await _domainEventDispatcher.DispatchEventsAsync(_cancellationToken);
        
        // Assert
        result.Should().BeSuccess();
        await _mediator.Received(2).Publish(Arg.Any<DomainEvent>(), _cancellationToken);
        _domainEventsAccessor.Received(1).ClearAllDomainEvents();
    }
    
    [Test]
    public async Task DispatchEventsAsync_WhenExceptionThrown_ShouldReturnFailureResult()
    {
        // Arrange
        _mediator.Configure().Publish(
                Arg.Any<FakeDomainEvent>(),
                Arg.Any<CancellationToken>())
            .ThrowsForAnyArgs(new Exception("Test Exception"));
        
        // Act
        var result = await _domainEventDispatcher.DispatchEventsAsync(_cancellationToken);
        
        // Assert
        result.Should().BeFailure().And.HaveError("Test Exception");
        _domainEventsAccessor.DidNotReceive().ClearAllDomainEvents();
    }

    internal record FakeDomainEvent(Guid Id) : DomainEvent;
}