using System.Reflection;
using Heyer.Storage.API.Endpoints;
using Heyer.Storage.API.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStorageStrategy(builder.Configuration.GetSection("StorageStrategy"));

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
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