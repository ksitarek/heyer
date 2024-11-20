using System.Reflection;
using Heyer.Storage.API.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStorageStrategy(builder.Configuration.GetSection("StorageStrategy"));

builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

await app.RunAsync();