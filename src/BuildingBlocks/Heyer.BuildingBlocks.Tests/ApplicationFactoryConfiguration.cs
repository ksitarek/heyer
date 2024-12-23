namespace Heyer.BuildingBlocks.Tests;

public static class ApplicationFactoryConfiguration
{
    public static readonly Dictionary<string, string?> InMemoryConfiguration = new();
    public static readonly Guid Tenant1Id = Guid.Parse("A62C048C-8E0F-41E2-84D4-BD061F9DDE97");
    public static readonly Guid Tenant2Id = Guid.Parse("0692183B-CE56-432D-88B5-B59280A678C5");

    public static void AddConfig(string key, string value) => InMemoryConfiguration.Add(key, value);

    public static void AddTenantConfig(Guid tenantId, string key, string value) =>
        InMemoryConfiguration.Add($"Companies:{tenantId}:{key}", value);
}