using Heyer.BuildingBlocks.Application.Authorization;
using Heyer.BuildingBlocks.Infrastructure;
using Heyer.BuildingBlocks.Infrastructure.Mediator;
using Heyer.BuildingBlocks.Infrastructure.Mediator.Middleware;
using Heyer.BuildingBlocks.Infrastructure.Modules;
using Heyer.Modules.JobBoard.Infrastructure;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var modules = new IModule[]
{
    new JobBoardModule(builder.Configuration)
};

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IDateTimeProvider, SystemDateTime>();
builder.Services.AddMediator(
    typeof(LoggingMiddleware<,>),
    typeof(ValidationMiddleware<,>),
    typeof(UnitOfWorkMiddleware<,>));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasPermission", policy =>
    {
        // policy.Requirements.Add(new HasPermissionAuthorizationRequirement());
        policy.AddAuthenticationSchemes("Bearer");
    });
    
});

builder.Services.AddScoped<IAuthorizationHandler, HasPermissionAuthorizationHandler>();

builder.AddModules(modules);

var app = builder.Build();

app.UseModules(modules);

await app.RunAsync();

public partial class Program {}