using FluentAssertions;
using FluentResults;
using FluentResults.Extensions.FluentAssertions;
using Heyer.BuildingBlocks.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;

namespace Heyer.BuildingBlocks.Infrastructure.Tests;

[Category("Unit")]
public class UnitOfWorkTests
{
    private DbContext _context;
    private IDomainEventDispatcher _domainEventDispatcher;
    private UnitOfWork _unitOfWork;
    private static CancellationToken _cancellationToken = CancellationToken.None;

    [SetUp]
    public void Setup()
    {
        _context = Substitute.For<DbContext>();
        _context.Configure().SaveChangesAsync(_cancellationToken)
            .Returns(3);
        
        _domainEventDispatcher = Substitute.For<IDomainEventDispatcher>();
        _domainEventDispatcher.Configure().DispatchEventsAsync(_cancellationToken)
            .Returns(Result.Ok());

        _unitOfWork = new UnitOfWork(_context, _domainEventDispatcher);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task UnitOfWork_ShouldSucceed_WhenCommitAsyncCalled()
    {
        // Arrange
        
        // Act
        var result = await _unitOfWork.CommitAsync(_cancellationToken);
        
        // Assert
        result.Should().BeSuccess().Which.Value.Should().Be(3);
        
        await _domainEventDispatcher.Received(1).DispatchEventsAsync(_cancellationToken);
        await _context.Received(1).SaveChangesAsync(_cancellationToken);
    }
    
    [Test]
    public async Task UnitOfWork_ShouldFail_WhenDispatchEventsAsyncFails()
    {
        // Arrange
        _domainEventDispatcher.Configure().DispatchEventsAsync(_cancellationToken)
            .Returns(Result.Fail("Error"));
        
        // Act
        var result = await _unitOfWork.CommitAsync(_cancellationToken);
        
        // Assert
        result.Should().BeFailure().Which.Errors.Should().ContainSingle().Which.Message.Should().Be("Error");
        
        await _domainEventDispatcher.Received(1).DispatchEventsAsync(_cancellationToken);
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
        result.Should().BeFailure().Which.Errors.Should().ContainSingle().Which.Message.Should().Be("Error.");
        
        await _domainEventDispatcher.Received(1).DispatchEventsAsync(_cancellationToken);
        await _context.Received(1).SaveChangesAsync(_cancellationToken);
    }
}