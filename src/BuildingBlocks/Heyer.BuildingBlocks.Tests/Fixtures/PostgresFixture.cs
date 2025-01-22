using System.Diagnostics;

namespace Heyer.BuildingBlocks.Tests.Fixtures;

public class PostgresFixture
{
    private readonly IPostgresFixtureProvider _provider;

    public PostgresFixture()
    {
        var dockerIsRunning = CheckForDocker();

        _provider = dockerIsRunning
            ? new DockerFixtureProvider()
            : new LocalPostgresFixtureProvider();
    }

    public string ConnectionString => _provider.ConnectionString;

    public async Task DisposeAsync() => await _provider.DisposeAsync();

    public async Task InitializeAsync() => await _provider.InitializeAsync();

    private bool CheckForDocker()
    {
        var processInfo = new ProcessStartInfo("docker", "ps");
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;

        int exitCode;
        using (var process = new Process())
        {
            process.StartInfo = processInfo;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit(1200000);
            if (!process.HasExited)
            {
                process.Kill();
            }

            exitCode = process.ExitCode;
            process.Close();
        }

        return exitCode == 0;
    }
}