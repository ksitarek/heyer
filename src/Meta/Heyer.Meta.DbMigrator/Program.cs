using Cocona;
using Heyer.Meta.DbMigrator.Commands.LoadSampleData;
using Heyer.Meta.DbMigrator.Commands.MigrateAllDatabases;
using Heyer.Meta.DbMigrator.Commands.MigrateHiringDb;
using Heyer.Meta.DbMigrator.Commands.MigrateJobBoardDb;
using Heyer.Meta.DbMigrator.Commands.MigrateStorageDb;
using Heyer.Meta.DbMigrator.Extensions;

var builder = CoconaApp.CreateBuilder();

builder
    .AddConfiguration()
    .AddLogging()
    .AddMediatR()
    .AddProviders();

var app = builder.Build();

app.AddCommand<MigrateAllDatabases>();
app.AddCommand<MigrateHiringDb>();
app.AddCommand<MigrateJobBoardDb>();
app.AddCommand<MigrateStorageDb>();

app.AddCommand<LoadSampleData>();

await app.RunAsync();