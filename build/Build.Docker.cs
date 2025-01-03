using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Polly;
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
    string _sqlEdgeContainerName = "Heyer-SqlEdge";
    int _sqlEdgePort = 41433;
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
        .DependsOn(BuildApiDockerImage, RunSqlEdge)
        .Executes(() =>
        {
            StopDockerContainer(_apiContainerName);

            string[] environmentVariables =
            [
                $"SqlServer__ConnectionString=Server=host.docker.internal,{_sqlEdgePort};Database=Heyer;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True",
                $"Scheduler__SqlServer__ConnectionString=Server=host.docker.internal,{_sqlEdgePort};Database=Scheduler;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True",
                // $"HiringModule__InboxOutbox__SqlServer__ConnectionString=Server=host.docker.internal,{_sqlEdgePort};Database=Hiring;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True",
                $"Companies__A62C048C-8E0F-41E2-84D4-BD061F9DDE97__SqlServer__ConnectionString=Server=host.docker.internal,{_sqlEdgePort};Database=A62C048C-8E0F-41E2-84D4-BD061F9DDE97;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True",
                $"Companies__0692183B-CE56-432D-88B5-B59280A678C5__SqlServer__ConnectionString=Server=host.docker.internal,{_sqlEdgePort};Database=0692183B-CE56-432D-88B5-B59280A678C5;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True"
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

    Target RunSqlEdge => _ => _
        .Executes(async () =>
        {
            StopDockerContainer(_sqlEdgeContainerName);

            string[] environmentVariables =
            {
                "ACCEPT_EULA=Y", "MSSQL_SA_PASSWORD=yourStrong(!)Password", "MSSQL_PID=Developer"
            };

            DockerTasks.DockerRun(x => x
                                      .SetImage("mcr.microsoft.com/azure-sql-edge:1.0.7")
                                      .SetName(_sqlEdgeContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_sqlEdgePort}:1433")
                                      .SetDetach(true)
                                      .SetEnv(environmentVariables));


            var databases = new[]
            {
                "Heyer", "Scheduler" /*"A62C048C-8E0F-41E2-84D4-BD061F9DDE97",
                "0692183B-CE56-432D-88B5-B59280A678C5"*/
            };

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(9, x => TimeSpan.FromSeconds(x))
                .ExecuteAsync(async () => await CreateDatabases(databases,
                                                                $"Server=localhost,{_sqlEdgePort};Database=master;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True"));
        });

    Target RunStorageApi => _ => _
        .DependsOn(BuildStorageApiDockerImage, RunSqlEdge)
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
                                          $"RegistryStrategy__SqlServerRegistry__ConnectionString=Server=host.docker.internal,{_sqlEdgePort};Database=Storage;User=sa;Password=yourStrong(!)Password;TrustServerCertificate=True"));
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

    private async Task CreateDatabases(string[] databases, string masterConnectionString)
    {
        await using var connection = new SqlConnection(masterConnectionString);
        await connection.OpenAsync();

        foreach (var dbName in databases)
        {
            var command =
                $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{dbName}') CREATE DATABASE [{dbName}];";

            await using var sqlCommand = new SqlCommand(command, connection);
            await sqlCommand.ExecuteNonQueryAsync();
        }

        await connection.CloseAsync();
    }

    private void StopDockerContainer(string containerName)
    {
        Log.Information("Stopping and removing existing container");

        DockerTasks.DockerRm(x => x
                                 .SetContainers(containerName)
                                 .SetForce(true));
    }
}