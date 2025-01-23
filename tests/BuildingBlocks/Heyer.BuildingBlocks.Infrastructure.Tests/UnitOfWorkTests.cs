using FluentResults;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Heyer.BuildingBlocks.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.BuildingBlocks.Infrastructure.Tests;

[Category("Unit")]
public class UnitOfWorkTests
{
    private static readonly CancellationToken _cancellationToken = CancellationToken.None;
    private DbContext _context;
    private IDomainEventDispatcher _domainEventDispatcher;
    private UnitOfWork _unitOfWork;

    [SetUp]
    public void Setup()
    {
        _context = Substitute.For<DbContext>();
        _context.Configure().SaveChangesAsync(_cancellationToken)
            .Returns(3);

        _domainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        _domainEventDispatcher.Configure().DispatchDomainEventsAsync(_cancellationToken)
            .Returns(Result.Ok());

        _unitOfWork = new UnitOfWork(_context, _domainEventDispatcher);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();

    [Test]
    public async Task UnitOfWork_ShouldFail_WhenDispatchEventsAsyncFails()
    {
        // Arrange
        _domainEventDispatcher.Configure().DispatchDomainEventsAsync(_cancellationToken)
            .Returns(Result.Fail("Error"));

        // Act
        var result = await _unitOfWork.CommitAsync(_cancellationToken);

        // Assert
        result.ShouldBeFailure("Error");

        await _domainEventDispatcher.Received(1).DispatchDomainEventsAsync(_cancellationToken);
        await _context.DidNotReceive().SaveChangesAsync(_cancellationToken);
    }

    [Test]
    public async Task UnitOfWork_ShouldFail_WhenSaveChangesAsyncFails()
    {
        // Arrange
        _context.Configure().SaveChangesAsync(_cancellationToken)
            .ThrowsAsync(new Exception("Error."));

        // Act
        var result = await _unitOfWork.CommitAsync(_cancellationToken);

        // Assert
        result.ShouldBeFailure("An error occurred while saving changes to the database.");
        result.ShouldHaveException<Exception>(x => x.Message == "Error.");

        await _domainEventDispatcher.Received(1).DispatchDomainEventsAsync(_cancellationToken);
        await _context.Received(1).SaveChangesAsync(_cancellationToken);
    }

    [Test]
    public async Task UnitOfWork_ShouldSucceed_WhenCommitAsyncCalled()
    {
        // Arrange

        // Act
        var result = await _unitOfWork.CommitAsync(_cancellationToken);

        // Assert
        result.ShouldBeSuccess(3);

        await _domainEventDispatcher.Received(1).DispatchDomainEventsAsync(_cancellationToken);
        await _context.Received(1).SaveChangesAsync(_cancellationToken);
    }
}