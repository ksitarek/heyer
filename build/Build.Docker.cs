using System.Threading;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Serilog;

public partial class Build
{
    [Parameter] readonly string _apiImageName = "heyer/api";

    [Parameter] readonly string _storageApiImageName = "heyer/storage-api";
    [Parameter] readonly string _webImageName = "heyer/web";

    string _apiContainerName = "Heyer-API";
    int _apiPort = 3001;

    [Parameter] string _apiTag = "local";
    string _mongoDbContainerName = "Heyer-MongoDB";
    int _mongoDbPort = 27117;
    string _storageApiContainerName = "Heyer-Storage-API";
    int _storageApiPort = 3002;

    [Parameter] string _storageApiTag = "local";
    string _webConfiguration = "local";
    string _webContainerName = "Heyer-Web";
    int _webPort = 4201;

    [Parameter] string _webTag = "local";

    Target BuildApiDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(_apiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{_apiImageName}:{_apiTag}")
                                        .SetNoCache(true));
        });

    Target BuildStorageApiDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(_storageApiPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{_storageApiImageName}:{_storageApiTag}")
                                        .SetNoCache(true));
        });

    Target BuildWebDockerImage => _ => _
        .Executes(() =>
        {
            DockerTasks.DockerBuild(x => x
                                        .SetProcessWorkingDirectory(RootDirectory)
                                        .SetFile(_webPath / "Dockerfile")
                                        .SetPath(".")
                                        .SetTag($"{_webImageName}:{_webTag}")
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

            string[] environmentVariables =
            [
                $"MongoDb__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}/?directConnection=true",
                $"Scheduler__MongoDb__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}/?directConnection=true",
                $"HiringModule__InboxOutbox__MongoDb__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}/?directConnection=true",
                $"Companies__A62C048C-8E0F-41E2-84D4-BD061F9DDE97__MongoDb__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}/?directConnection=true",
                $"Companies__0692183B-CE56-432D-88B5-B59280A678C5__MongoDb__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}/?directConnection=true"
            ];

            DockerTasks.DockerRun(x => x
                                      .SetImage($"{_apiImageName}:{_apiTag}")
                                      .SetName(_apiContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_apiPort}:8080")
                                      .SetDetach(true)
                                      .SetEnv(environmentVariables)
            );
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
                                      .SetArgs("--replSet=rs0")
                                      .SetDetach(true));

            Thread.Sleep(5000);

            DockerTasks.DockerExec(x => x
                                       .SetContainer(_mongoDbContainerName)
                                       .SetCommand("mongosh")
                                       .SetArgs("--quiet", "--eval", "\"rs.initiate();\""));
        });

    Target RunStorageApi => _ => _
        .DependsOn(BuildStorageApiDockerImage, RunMongoDb)
        .Executes(() =>
        {
            StopDockerContainer(_storageApiContainerName);

            DockerTasks.DockerRun(x => x
                                      .SetImage($"{_storageApiImageName}:{_storageApiTag}")
                                      .SetName(_storageApiContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_storageApiPort}:8080")
                                      .SetDetach(true)
                                      .SetEnv(
                                          $"RegistryStrategy__MongoDbRegistry__ConnectionString=mongodb://host.docker.internal:{_mongoDbPort}/?directConnection=true"));
        });

    Target RunWeb => _ => _
        .DependsOn(BuildWebDockerImage)
        .Executes(() =>
        {
            StopDockerContainer(_webContainerName);

            DockerTasks.DockerRun(x => x
                                      .SetImage($"{_webImageName}:{_webTag}")
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