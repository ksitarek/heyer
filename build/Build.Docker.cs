using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;

public partial class Build
{
    [Parameter] readonly string ApiImageName = "heyer/api";

    [Parameter] readonly string StorageApiImageName = "heyer/storage-api";
    string _apiContainerName = "Heyer-API";
    int _apiPort = 3001;
    string _mongoDbContainerName = "Heyer-MongoDB";
    int _mongoDbPort = 27117;
    string _storageApiContainerName = "Heyer-Storage-API";
    int _storageApiPort = 3002;
    [Parameter] string ApiTag = "local";

    [Parameter] string StorageApiTag = "local";

    Target BuildApiDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(ApiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{ApiImageName}:{ApiTag}"));
        });

    Target BuildStorageApiDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(StorageApiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{StorageApiImageName}:{StorageApiTag}"));
        });

    Target RunAll => _ => _
        .DependsOn(RunApi, RunStorageApi)
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