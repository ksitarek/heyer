using System.Diagnostics;
using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Tests;
using Heyer.Modules.Hiring.Infrastructure;
using Heyer.Modules.JobBoard.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.API.Tests.Utils;

internal static class AsyncHelper
{
    public static Guid[] Companies =
    [
        ApplicationFactoryConfiguration.Client1Id,
        ApplicationFactoryConfiguration.Client2Id
    ];

    public static IServiceScope[] Scopes =
    [
        HiringModuleCompositionRoot.CreateScope(),
        JobBoardModuleCompositionRoot.CreateScope()
    ];

    public static TimeSpan TimeOut = TimeSpan.FromSeconds(10);

    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public static async Task AssertAllMessagesProcessed(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync();
        var sw = Stopwatch.StartNew();

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var allMessagesProcessed = true;

                foreach (var scope in Scopes)
                {
                    foreach (var company in Companies)
                    {
                        var hasAnyMessages = await CheckAnyMessages(scope, company);

                        if (hasAnyMessages)
                        {
                            allMessagesProcessed = false;
                            break;
                        }
                    }
                }

                if (allMessagesProcessed)
                {
                    break;
                }

                if (sw.Elapsed > TimeOut)
                {
                    throw new TimeoutException("Not all messages were processed in time");
                }

                await Task.Delay(2000, cancellationToken);
            }
        }
        finally
        {
            foreach (var scope in Scopes)
            {
                scope.Dispose();
            }

            _semaphore.Release();
        }
    }

    private static async Task<bool> CheckAnyMessages(IServiceScope scope,
                                                     Guid company)
    {
        var valueUserDataProvider =
            scope.ServiceProvider.GetRequiredService<IUserDataProvider>() as ValueUserDataProvider;

        valueUserDataProvider!.SetExecutionContext(Guid.Empty, company, string.Empty);

        var inboxContext = scope.ServiceProvider.GetRequiredService<IInboxStore>();

        var outboxContext = scope.ServiceProvider.GetRequiredService<IOutboxStore>();

        var hasAnyInboxMessages = await inboxContext.GetUnprocessedMessages();

        var hasAnyOutboxMessages = await outboxContext.GetUnprocessedMessages();

        return hasAnyInboxMessages.Any() || hasAnyOutboxMessages.Any();
    }
}