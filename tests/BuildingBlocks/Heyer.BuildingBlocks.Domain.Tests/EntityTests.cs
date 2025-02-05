using Heyer.BuildingBlocks.Tests.Extensions;
using Shouldly;

namespace Heyer.BuildingBlocks.Domain.Tests;

[Category("Unit")]
public class EntityTests
{
    [Test]
    public void TestEntity_ShouldHaveOneDomainEvent()
    {
        // Arrange
        var id = Guid.CreateVersion7();

        // Act
        var testEntity = new TestEntity(id);

        // Assert
        testEntity.DomainEvents.Count.ShouldBe(1);

        var @event = testEntity.DomainEvents.First() as TestEntityCreated;
        @event.ShouldBeOfType<TestEntityCreated>();
        @event.EventId.ShouldNotBeEmpty();
        @event.OccurredOn.ShouldBeWithin(TimeSpan.FromMilliseconds(10));
        @event.TestEntityId.ShouldBe(id);
    }

    [Test]
    public void TestEntity_ShouldRemoveAllEvents_WhenClearDomainEventsCalled()
    {
        // Arrange
        var id = Guid.CreateVersion7();

        // Act
        var testEntity = new TestEntity(id);
        testEntity.ClearDomainEvents();

        // Assert
        testEntity.DomainEvents.Count.ShouldBe(0);
    }

    internal class TestEntity : Entity
    {
        public TestEntity(Guid id) => AddDomainEvent(new TestEntityCreated(id));
    }

    internal record TestEntityCreated(Guid TestEntityId) : DomainEvent;
}