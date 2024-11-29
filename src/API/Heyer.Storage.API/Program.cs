using System.Reflection;
using FluentValidation;
using Heyer.Storage.API.Endpoints;
using Heyer.Storage.API.Extensions;
using Heyer.Storage.API.Middleware;
using Heyer.Storage.API.Providers.Registry;
using Heyer.Storage.API.Providers.Storage;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder.Configuration.GetSection("Jwt"));
builder.Services.AddStorageStrategy(builder.Configuration.GetSection("StorageStrategy"));
builder.Services.AddRegistryStrategy(builder.Configuration.GetSection("RegistryStrategy"));

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorLoggingMiddleware<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorValidationMiddleware<,>));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();

var app = builder.Build();

app.UseAntiforgery();

app.MapEndpoints();

await app.RunAsync();

// For Integration Testing
namespace Heyer.Storage.API
{
    public partial class Program
    {
    };
}