using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

public partial class Build
{
    [Parameter] readonly string StorageApiImageName = "heyer/storage-api";

    [Parameter] readonly string StorageApiTag = "local";

    Target RunStorageApi => _ => _
        .DependsOn(StorageApiDockerBuild)
        .Executes(() =>
        {
            DockerTasks.DockerRun(x => x
                                      .SetImage($"{StorageApiImageName}:{StorageApiTag}")
                                      .SetName("Heyer-Storage-API")
                                      .SetRm(true)
                                      .SetPublish("3002:8080")
                                      .SetDetach(true));
        });

    Target StorageApiDockerBuild => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(StorageApiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{StorageApiImageName}:{StorageApiTag}"));
        });
}