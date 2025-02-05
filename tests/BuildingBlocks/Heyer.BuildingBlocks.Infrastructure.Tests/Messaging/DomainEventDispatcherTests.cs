using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Domain;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Tests.Extensions;
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
        new FakeDomainEvent(Guid.CreateVersion7()), new FakeDomainEvent(Guid.CreateVersion7())
    };

    private DomainEventDispatcher _domainEventDispatcher = null!;
    private DomainEventNotificationsRegistry _domainEventNotificationsRegistry = null!;
    private IDomainEventsAccessor _domainEventsAccessor = null!;
    private IMediator _mediator = null!;
    private IOutboxStore _outboxStore = null!;
    private ValueUserDataProvider _userDataProvider = null!;

    [Test]
    public async Task DispatchEventsAsync_WhenCalled_ShouldPublishAllDomainEvents()
    {
        // Act
        var result = await _domainEventDispatcher.DispatchDomainEventsAsync(_cancellationToken);

        // Assert
        result.ShouldBeSuccess();
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
        result.ShouldBeFailure("Failed to dispatch domain events.");
        result.ShouldHaveException<Exception>(x => x.Message == "Test Exception");

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
        public required DbSet<OutboxMessage> OutboxMessages { get; init; }
    }

    internal record FakeDomainEvent(Guid Id) : DomainEvent;
}