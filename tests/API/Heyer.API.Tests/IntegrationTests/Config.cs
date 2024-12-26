using System.Diagnostics.CodeAnalysis;

namespace Heyer.API.Tests.IntegrationTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Config
{
    public const string HiringModule_InboxOutbox_MongoDb_ConnectionString =
        "HiringModule:InboxOutbox:MongoDb:ConnectionString";

    public const string HiringModule_InboxOutbox_MongoDb_DatabaseName = "HiringModule:InboxOutbox:MongoDb:DatabaseName";
    public const string Jwt_Secret = "Jwt:Secret";
    public const string Jwt_ValidateAudience = "Jwt:ValidateAudience";
    public const string Jwt_ValidateIssuer = "Jwt:ValidateIssuer";
    public const string Jwt_ValidateLifetime = "Jwt:ValidateLifetime";
    public const string Jwt_ValidAudience = "Jwt:ValidAudience";
    public const string Jwt_ValidIssuer = "Jwt:ValidIssuer";

    public const string MongoDb_ConnectionString = "MongoDb:ConnectionString";
    public const string MongoDb_DatabaseName = "MongoDb:DatabaseName";

    public const string Scheduler_MongoDb_ConnectionString = "Scheduler:MongoDb:ConnectionString";
    public const string Scheduler_MongoDb_DatabaseName = "Scheduler:MongoDb:DatabaseName";
}