using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Serilog;

public partial class Build
{
    readonly AbsolutePath E2ETestsProject = RootDirectory / "tests/E2E/Heyer.E2E.Tests";

    Target PrepareE2EEnv => _ => _
        .Triggers(RunAll)
        .Executes(() =>
        {
            ApiTag = "E2E";
            StorageApiTag = "E2E";

            _mongoDbPort = 27217;
            _mongoDbContainerName = "Heyer-E2E-MongoDB";

            _apiPort = 43001;
            _apiContainerName = "Heyer-E2E-API";

            _storageApiPort = 43002;
            _storageApiContainerName = "Heyer-E2E-Storage-API";
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
                                           .SetProjectFile(E2ETestsProject));
            }
            finally
            {
                StopDockerContainer(_mongoDbContainerName);
                StopDockerContainer(_apiContainerName);
                StopDockerContainer(_storageApiContainerName);
            }
        });
}