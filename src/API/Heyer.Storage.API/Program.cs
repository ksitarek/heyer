using Heyer.Storage.API.Providers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddStorageStrategy(builder.Configuration.GetSection("StorageStrategy"));

var app = builder.Build();

await app.RunAsync();