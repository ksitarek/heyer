using FluentResults;
using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Json;
using MediatR;
using Serilog;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

public abstract class GenericInboxProcessingJob
{
    protected IInboxStore? _inboxStore;
    protected IMediator? _mediator;
    protected IUserDataProvider? _userDataProvider;

    [DisableConcurrentExecution(100)]
    public async Task<Result> Handle()
    {
        if (_inboxStore == null)
        {
            Log.Warning("{TypeName}: Inbox context is not set", GetType().Namespace);
            return Result.Fail("Inbox context is not set");
        }

        if (_mediator == null)
        {
            Log.Warning("{TypeName}: Mediator is not set", GetType().Namespace);
            return Result.Fail("Mediator is not set");
        }

        var messages = await _inboxStore.GetUnprocessedMessages();

        if (messages.Count > 0)
        {
            Log.Debug("{TypeName}: Processing inbox messages. Count: {Cnt}", GetType().Namespace, messages.Count);
        }

        foreach (var message in messages)
        {
            await ProcessMessage(message);
        }

        Log.Debug("{TypeName}: Processed inbox messages", GetType().Namespace);

        return Result.Ok();
    }

    private async Task ProcessMessage(InboxMessage message)
    {
        var messageAssembly = AppDomain.CurrentDomain.GetAssemblies();

        var type = messageAssembly
            .SelectMany(x => x.GetTypes())
            .FirstOrDefault(x => x.FullName == message.Type);

        if (type != null)
        {
            var command = message.Data.Deserialize(type);

            if (command is IDomainEventNotification domainNotification)
            {
                var valueUserDataProvider = _userDataProvider as ValueUserDataProvider;

                valueUserDataProvider!.SetExecutionContext(domainNotification.ExecutionContext.UserId,
                                                           domainNotification.ExecutionContext.CompanyId,
                                                           domainNotification.ExecutionContext.CompanyName);
            }

            try
            {
                await _mediator!.Publish(command!);
                await _inboxStore!.SetProcessedAt(message.Id, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{TypeName}: Error processing inbox message", GetType().Namespace);
            }
        }
    }
}