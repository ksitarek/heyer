namespace Heyer.BuildingBlocks.Tests.Fixtures;

public interface IPostgresFixtureProvider
{
    string ConnectionString { get; }
    Task DisposeAsync();
    Task InitializeAsync();
}