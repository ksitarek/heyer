using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure.Integration.Persistence;
using Heyer.BuildingBlocks.Infrastructure.Integration.Processing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Heyer.Modules.Hiring.Infrastructure.Integration;

internal class CompanyHiringInboxProcessingJob : GenericInboxProcessingJob
{
    public CompanyHiringInboxProcessingJob(IServiceScope scope)
    {
        _mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        _userDataProvider = scope.ServiceProvider.GetRequiredService<IUserDataProvider>();
        _inboxStore = scope.ServiceProvider.GetRequiredService<IInboxStore>();
    }
}