using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;

public partial class Build
{
    [Parameter] readonly string ApiImageName = "heyer/api";

    [Parameter] readonly string StorageApiImageName = "heyer/storage-api";
    [Parameter] readonly string WebImageName = "heyer/web";

    string _apiContainerName = "Heyer-API";
    int _apiPort = 3001;
    string _mongoDbContainerName = "Heyer-MongoDB";
    int _mongoDbPort = 27117;
    string _storageApiContainerName = "Heyer-Storage-API";
    int _storageApiPort = 3002;
    string _webConfiguration = "local";
    string _webContainerName = "Heyer-Web";
    int _webPort = 4201;

    [Parameter] string ApiTag = "local";

    [Parameter] string StorageApiTag = "local";

    [Parameter] string WebTag = "local";

    Target BuildApiDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(ApiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{ApiImageName}:{ApiTag}")
                                        .SetNoCache(true));
        });

    Target BuildStorageApiDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(StorageApiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{StorageApiImageName}:{StorageApiTag}")
                                        .SetNoCache(true));
        });

    Target BuildWebDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(WebPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{WebImageName}:{WebTag}")
                                        .SetBuildArg($"CONFIGURATION={_webConfiguration}")
                                        .SetNoCache(true));
        });

    Target RunAll => _ => _
        .DependsOn(RunBackend, RunWeb)
        .Executes(() =>
        {
        });

    Target RunApi => _ => _
        .DependsOn(BuildApiDockerImage, RunMongoDb)
        .Executes(() =>
        {
            StopDockerContainer(_apiContainerName);

            DockerTasks.DockerRun(x => x
                                      .SetImage($"{ApiImageName}:{ApiTag}")
                                      .SetName(_apiContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_apiPort}:8080")
                                      .SetDetach(true)
                                      .SetEnv(
                                          $"MongoDb__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}"));
        });

    Target RunBackend => _ => _
        .DependsOn(RunApi, RunStorageApi)
        .Executes(() =>
        {
        });

    Target RunMongoDb => _ => _
        .Executes(() =>
        {
            StopDockerContainer(_mongoDbContainerName);

            DockerTasks.DockerRun(x => x
                                      .SetImage("mongo:8")
                                      .SetName(_mongoDbContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_mongoDbPort}:27017")
                                      .SetDetach(true));
        });

    Target RunStorageApi => _ => _
        .DependsOn(BuildStorageApiDockerImage, RunMongoDb)
        .Executes(() =>
        {
            StopDockerContainer(_storageApiContainerName);

            DockerTasks.DockerRun(x => x
                                      .SetImage($"{StorageApiImageName}:{StorageApiTag}")
                                      .SetName(_storageApiContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_storageApiPort}:8080")
                                      .SetDetach(true)
                                      .SetEnv(
                                          $"RegistryStrategy__MongoDbRegistry__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}"));
        });

    Target RunWeb => _ => _
        .DependsOn(BuildWebDockerImage)
        .Executes(() =>
        {
            StopDockerContainer(_webContainerName);

            DockerTasks.DockerRun(x => x
                                      .SetImage($"{WebImageName}:{WebTag}")
                                      .SetName(_webContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_webPort}:4000")
                                      .SetDetach(true));
        });

    private void StopDockerContainer(string containerName)
    {
        var ps = DockerTasks.DockerPs(x => x
                                          .SetFilter($"name={containerName}")
                                          .SetFormat("{{.ID}}"));

        if (ps.Count == 0)
        {
            return;
        }

        Log.Information("Stopping and removing existing container");

        DockerTasks.DockerStop(x => x
                                   .SetContainers(containerName));
    }
}