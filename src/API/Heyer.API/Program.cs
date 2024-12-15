using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.Candidates.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var modules = new IModule[]
{
    new CandidatesModule(builder.Configuration)
};

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IDateTimeProvider, SystemDateTime>();
builder.Services.AddMediator(
    typeof(LoggingMiddleware<,>),
    typeof(ValidationMiddleware<,>),
    typeof(UnitOfWorkMiddleware<,>));

builder.AddModules(modules);

var app = builder.Build();

app.UseModules(modules);

await app.RunAsync();