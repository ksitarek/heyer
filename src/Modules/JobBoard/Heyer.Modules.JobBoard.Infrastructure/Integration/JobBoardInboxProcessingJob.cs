using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.JobBoard.Infrastructure.Integration;

internal class JobBoardInboxProcessingJob : GenericInboxProcessingJob, IDisposable
{
    private readonly IServiceScope _scope;

    public JobBoardInboxProcessingJob()
    {
        _scope = JobBoardModuleCompositionRoot.CreateScope();
        _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();
        _inboxStore = _scope.ServiceProvider.GetRequiredService<IInboxStore>();
        _userDataProvider = _scope.ServiceProvider.GetRequiredService<IUserDataProvider>();
    }

    public void Dispose() => _scope.Dispose();
}