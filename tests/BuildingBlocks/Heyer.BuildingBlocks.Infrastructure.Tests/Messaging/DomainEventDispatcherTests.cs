using FluentAssertions;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Domain;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.BuildingBlocks.Infrastructure.Tests.Messaging;

[Category("Unit")]
public class DomainEventDispatcherTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private readonly List<DomainEvent> _domainEvents = new()
    {
        new FakeDomainEvent(Guid.NewGuid()), new FakeDomainEvent(Guid.NewGuid())
    };

    private DomainEventDispatcher _domainEventDispatcher;
    private DomainEventNotificationsRegistry _domainEventNotificationsRegistry;
    private IDomainEventsAccessor _domainEventsAccessor;
    private IMediator _mediator;
    private IOutboxStore _outboxStore;
    private ValueUserDataProvider _userDataProvider;

    [Test]
    public async Task DispatchEventsAsync_WhenCalled_ShouldPublishAllDomainEvents()
    {
        // Act
        var result = await _domainEventDispatcher.DispatchDomainEventsAsync(_cancellationToken);

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
        var result = await _domainEventDispatcher.DispatchDomainEventsAsync(_cancellationToken);

        // Assert
        result.Should().BeFailure().And.HaveError("Failed to dispatch domain events.")
            .Which.HasException<Exception>(x => x.Message == "Test Exception").Should().BeTrue();

        _domainEventsAccessor.DidNotReceive().ClearAllDomainEvents();
    }

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

        _domainEventNotificationsRegistry = new DomainEventNotificationsRegistry();

        _outboxStore = Substitute.For<IOutboxStore>();

        _userDataProvider = new ValueUserDataProvider();

        _domainEventDispatcher =
            new DomainEventDispatcher(_mediator,
                                      _domainEventsAccessor,
                                      _domainEventNotificationsRegistry,
                                      _outboxStore,
                                      _userDataProvider);
    }

    internal class OutboxContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; init; }
    }

    internal record FakeDomainEvent(Guid Id) : DomainEvent;
}