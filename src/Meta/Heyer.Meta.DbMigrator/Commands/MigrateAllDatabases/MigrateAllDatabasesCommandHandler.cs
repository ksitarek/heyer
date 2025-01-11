using MediatR;

namespace Heyer.Meta.DbMigrator.Commands.MigrateAllDatabases;

internal class MigrateAllDatabasesCommandHandler : IRequestHandler<MigrateAllDatabases>
{
    private readonly IMediator _mediator;

    public MigrateAllDatabasesCommandHandler(IMediator mediator) => _mediator = mediator;

    public Task Handle(MigrateAllDatabases request, CancellationToken cancellationToken)
    {
        var tasks = new[]
        {
            _mediator.Send(new MigrateSchedulerDb.MigrateSchedulerDb(), cancellationToken),
            _mediator.Send(new MigrateStorageDb.MigrateStorageDb(), cancellationToken),
            _mediator.Send(new MigrateHiringDb.MigrateHiringDb(), cancellationToken),
            _mediator.Send(new MigrateJobBoardDb.MigrateJobBoardDb(), cancellationToken)
        };
        return Task.WhenAll(tasks);
    }
}