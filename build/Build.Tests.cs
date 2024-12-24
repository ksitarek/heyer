using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Serilog;

public partial class Build
{
    readonly AbsolutePath _e2ETestsProject = RootDirectory / "tests/E2E/Heyer.E2E.Tests";

    Target PrepareE2EEnv => _ => _
        .Triggers(RunAll)
        .Executes(() =>
        {
            _apiTag = "E2E";
            _storageApiTag = "E2E";

            _mongoDbPort = 27217;
            _mongoDbContainerName = "Heyer-E2E-MongoDB";

            _apiPort = 43001;
            _apiContainerName = "Heyer-E2E-API";

            _storageApiPort = 43002;
            _storageApiContainerName = "Heyer-E2E-Storage-API";

            _webPort = 44001;
            _webContainerName = "Heyer-E2E-Web";
            _webConfiguration = "e2e";
        });

    Target RunE2E => _ => _
        .DependsOn(PrepareE2EEnv)
        .After(RunAll)
        .Executes(() =>
        {
            try
            {
                Log.Information("Running E2E tests");

                DotNetTasks.DotNetTest(x => x
                                           .SetProjectFile(_e2ETestsProject));
            }
            finally
            {
                StopDockerContainer(_webContainerName);
                StopDockerContainer(_mongoDbContainerName);
                StopDockerContainer(_apiContainerName);
                StopDockerContainer(_storageApiContainerName);
            }
        });
}