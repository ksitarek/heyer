using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
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
    int _posrgesPort = 41433;
    string _sqlEdgeContainerName = "Heyer-SqlEdge";
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
        .DependsOn(BuildApiDockerImage, RunPostgres)
        .Executes(() =>
        {
            StopDockerContainer(_apiContainerName);

            //

            string[] environmentVariables =
            [
                $"Npgsql__ConnectionString=Host=localhost;Port={_posrgesPort};Username=postgres;Password=yourStrong(!)Password;Database=heyer;TrustServerCertificate=True",
                $"Scheduler__Npgsql__ConnectionString=Host=localhost;Port={_posrgesPort};Username=postgres;Password=yourStrong(!)Password;Database=scheduler;TrustServerCertificate=True",
                // $"HiringModule__InboxOutbox__Npgsql__ConnectionString=Host=localhost;Port={_posrgesPort};Username=postgres;Password=yourStrong(!)Password;Database=hiring;TrustServerCertificate=True",
                $"Companies__A62C048C-8E0F-41E2-84D4-BD061F9DDE97__Npgsql__ConnectionString=Host=localhost;Port={_posrgesPort};Username=postgres;Password=yourStrong(!)Password;Database=C_A62C048C-8E0F-41E2-84D4-BD061F9DDE97;TrustServerCertificate=True",
                $"Companies__0692183B-CE56-432D-88B5-B59280A678C5__Npgsql__ConnectionString=Host=localhost;Port={_posrgesPort};Username=postgres;Password=yourStrong(!)Password;Database=C_0692183B-CE56-432D-88B5-B59280A678C5;TrustServerCertificate=True"
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

    Target RunPostgres => _ => _
        .Executes(async () =>
        {
            StopDockerContainer(_sqlEdgeContainerName);

            var password = "yourStrong(!)Password";
            string[] environmentVariables = { $"POSTGRES_PASSWORD={password}" };

            DockerTasks.DockerRun(x => x
                                      .SetImage("postgres:17")
                                      .SetName(_sqlEdgeContainerName)
                                      .SetRm(true)
                                      .SetPublish($"{_posrgesPort}:5432")
                                      .SetDetach(true)
                                      .SetEnv(environmentVariables));

            var connectionString =
                $"Host=localhost:{_posrgesPort};Username=postgres;Password={password};Database=postgres;TrustServerCertificate=True";

            await WaitForPostgresDb(connectionString);

            DotNetTasks.DotNetRun(_ => _
                                      .SetProjectFile(_dbMigratorPath)
                                      .SetApplicationArguments("migrate-all-databases"));
        });

    Target RunStorageApi => _ => _
        .DependsOn(BuildStorageApiDockerImage, RunPostgres)
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
                                          $"RegistryStrategy__NpgsqlRegistry__ConnectionString=Host=localhost;Port={_posrgesPort};Username=postgres;Password=yourStrong(!)Password;Database=storage;TrustServerCertificate=True"));
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
        Log.Information("Stopping and removing existing container");

        DockerTasks.DockerRm(x => x
                                 .SetContainers(containerName)
                                 .SetForce(true));
    }

    private async Task WaitForPostgresDb(string connectionString)
    {
        var sw = new Stopwatch();
        sw.Start();

        var i = 0;
        while (true)
        {
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                await connection.CloseAsync();

                Log.Information("Postgres is running");

                return;
            }
            catch
            {
                Log.Information("Waiting for Postgres to start... {i}ms", i += 300);

                var r = DockerTasks.DockerPs(_ => new DockerPsSettings()
                                                 .SetFilter($"name={_sqlEdgeContainerName}")
                                                 .SetQuiet(true)
                                                 .DisableProcessLogOutput());

                if (r.Count == 0)
                {
                    Log.Fatal("Postgres container crashed.");
                    throw;
                }

                if (sw.Elapsed > TimeSpan.FromSeconds(10))
                {
                    throw new TimeoutException("Postgres did not start in time.");
                }

                await Task.Delay(TimeSpan.FromMilliseconds(300));
            }
        }
    }
}