using FluentAssertions;

namespace Heyer.BuildingBlocks.Domain.Tests;

[Category("Unit")]
public class EntityTests
{
    [Test]
    public void TestEntity_ShouldHaveOneDomainEvent()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var testEntity = new TestEntity(id);

        // Assert
        var @event = testEntity.DomainEvents.Should().HaveCount(1).And.Subject.Single();
        @event.Should().BeOfType<TestEntityCreated>();
        @event.As<TestEntityCreated>().EventId.Should().NotBeEmpty();
        @event.As<TestEntityCreated>().OccurredOn.Should().BeWithin(TimeSpan.FromMilliseconds(10));
        @event.As<TestEntityCreated>().TestEntityId.Should().Be(id);
    }

    [Test]
    public void TestEntity_ShouldRemoveAllEvents_WhenClearDomainEventsCalled()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var testEntity = new TestEntity(id);
        testEntity.ClearDomainEvents();

        // Assert
        var @event = testEntity.DomainEvents.Should().HaveCount(0);
    }

    internal class TestEntity : Entity
    {
        public TestEntity(Guid id) => AddDomainEvent(new TestEntityCreated(id));
    }

    internal record TestEntityCreated(Guid TestEntityId) : DomainEvent;
}