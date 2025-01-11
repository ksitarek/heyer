using Cocona;
using Cocona.Builder;
using MediatR;

namespace Heyer.Meta.DbMigrator.Extensions;

internal static class AddCommandExtensions
{
    public static CommandConventionBuilder AddCommand<TCommand>(this CoconaApp app)
        where TCommand : IRequest, new() =>
        app.AddCommand(typeof(TCommand).Name,
                       async (IMediator mediator, CoconaAppContext ctx) =>
                       {
                           await mediator.Send(new TCommand(),
                                               ctx.CancellationToken);
                       });
}