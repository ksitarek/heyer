using System.Reflection;
using FluentValidation;
using Heyer.Storage.API.Endpoints;
using Heyer.Storage.API.Middleware;
using Heyer.Storage.API.Providers;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStorageStrategy(builder.Configuration.GetSection("StorageStrategy"));

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MediatorValidationMiddleware<,>));
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAntiforgery();

var app = builder.Build();

app.UseAntiforgery();

app.MapEndpoints();

await app.RunAsync();

// For Integration Testing
public partial class Program
{
};