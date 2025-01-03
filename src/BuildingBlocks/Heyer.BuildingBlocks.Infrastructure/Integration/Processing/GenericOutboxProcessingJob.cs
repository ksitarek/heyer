using System.Text.Json;
using FluentResults;
using Hangfire;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Application.Notifications;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using MediatR;
using Serilog;

namespace Heyer.BuildingBlocks.Infrastructure.Integration.Processing;

public abstract class GenericOutboxProcessingJob
{
    protected IMediator _mediator;
    protected IOutboxStore _outboxStore;
    protected IUserDataProvider _userDataProvider;

    protected GenericOutboxProcessingJob(IMediator mediator,
                                         IUserDataProvider userDataProvider,
                                         IOutboxStore outboxStore)
    {
        _mediator = mediator;
        _userDataProvider = userDataProvider;
        _outboxStore = outboxStore;
    }

    [DisableConcurrentExecution(100)]
    public async Task<Result> Handle()
    {
        var messages = await _outboxStore.GetUnprocessedMessages();

        if (messages.Count > 0)
        {
            Log.Information("{TypeName}: Processing outbox messages. Count: {Cnt}",
                            GetType().Namespace,
                            messages.Count);
        }

        foreach (var message in messages)
        {
            await ProcessMessage(message);
        }

        Log.Debug("{TypeName}: Processed outbox messages", GetType().Namespace);

        return Result.Ok();
    }

    private async Task ProcessMessage(OutboxMessage message)
    {
        var messageAssembly = AppDomain.CurrentDomain.GetAssemblies();

        var type = messageAssembly
            .SelectMany(x => x.GetTypes())
            .FirstOrDefault(x => x.FullName == message.Type);

        if (type != null)
        {
            var command = JsonSerializer.Deserialize(message.Data, type);

            if (command is IDomainEventNotification domainNotification)
            {
                var valueUserDataProvider = _userDataProvider as ValueUserDataProvider;

                valueUserDataProvider!.SetExecutionContext(domainNotification.ExecutionContext.UserId,
                                                           domainNotification.ExecutionContext.CompanyId,
                                                           domainNotification.ExecutionContext.CompanyName);
            }

            await _mediator.Publish(command!);

            await _outboxStore.SetProcessedAt(message.Id, DateTime.UtcNow);
        }
    }
}