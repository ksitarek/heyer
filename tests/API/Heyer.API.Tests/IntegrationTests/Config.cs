using System.Diagnostics.CodeAnalysis;

namespace Heyer.API.Tests.IntegrationTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Config
{
    public const string HiringModule_InboxOutbox_SqlServer_ConnectionString =
        "HiringModule:InboxOutbox:SqlServer:ConnectionString";

    public const string HiringModule_InboxOutbox_SqlServer_DatabaseName =
        "HiringModule:InboxOutbox:SqlServer:DatabaseName";

    public const string Jwt_Secret = "Jwt:Secret";
    public const string Jwt_ValidateAudience = "Jwt:ValidateAudience";
    public const string Jwt_ValidateIssuer = "Jwt:ValidateIssuer";
    public const string Jwt_ValidateLifetime = "Jwt:ValidateLifetime";
    public const string Jwt_ValidAudience = "Jwt:ValidAudience";
    public const string Jwt_ValidIssuer = "Jwt:ValidIssuer";

    public const string Scheduler_SqlServer_ConnectionString = "Scheduler:SqlServer:ConnectionString";
    public const string Scheduler_SqlServer_DatabaseName = "Scheduler:SqlServer:DatabaseName";

    public const string SqlServer_ConnectionString = "SqlServer:ConnectionString";
    public const string SqlServer_DatabaseName = "SqlServer:DatabaseName";
}