using System.Diagnostics.CodeAnalysis;

namespace Heyer.API.Tests.IntegrationTests;

[SuppressMessage("ReSharper", "InconsistentNaming")]
internal static class Config
{
    public const string HiringModule_InboxOutbox_Npgsql_ConnectionString =
        "HiringModule:InboxOutbox:Npgsql:ConnectionString";

    public const string HiringModule_InboxOutbox_Npgsql_DatabaseName =
        "HiringModule:InboxOutbox:Npgsql:DatabaseName";

    public const string Jwt_Secret = "Jwt:Secret";
    public const string Jwt_ValidateAudience = "Jwt:ValidateAudience";
    public const string Jwt_ValidateIssuer = "Jwt:ValidateIssuer";
    public const string Jwt_ValidateLifetime = "Jwt:ValidateLifetime";
    public const string Jwt_ValidAudience = "Jwt:ValidAudience";
    public const string Jwt_ValidAuthority = "Jwt:ValidAuthority";
    public const string Jwt_ValidIssuer = "Jwt:ValidIssuer";

    public const string Npgsql_ConnectionString = "Npgsql:ConnectionString";
    public const string Npgsql_DatabaseName = "Npgsql:DatabaseName";

    public const string Scheduler_Npgsql_ConnectionString = "Scheduler:Npgsql:ConnectionString";
    public const string Scheduler_Npgsql_DatabaseName = "Scheduler:Npgsql:DatabaseName";
}