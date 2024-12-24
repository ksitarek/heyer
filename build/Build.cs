using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Serilog;

// ReSharper disable AllUnderscoreLocalParameterName

partial class Build : NukeBuild
{
    readonly AbsolutePath _apiPath = RootDirectory / "src/API/Heyer.API";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution _solution;
    readonly AbsolutePath _storageApiPath = RootDirectory / "src/API/Heyer.Storage.API";
    readonly AbsolutePath _webPath = RootDirectory / "web";

    public Build() =>
        DockerTasks.DockerLogger = (_, m) =>
            Log.Information(m);

    Target Clean => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(t => t
                                        .SetConfiguration(_configuration)
                                        .SetProject(_solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(t => t
                                        .SetConfiguration(_configuration)
                                        .SetProjectFile(_solution)
                                        .SetNoRestore(true));
        });

    Target IntegrationTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(_configuration)
                                       .SetProjectFile(_solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true)
                                       .SetFilter("TestCategory=Integration"));
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(t => t.SetProjectFile(_solution));
        });

    Target RunAllTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(_configuration)
                                       .SetProjectFile(_solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true));
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(_configuration)
                                       .SetProjectFile(_solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true)
                                       .SetFilter("TestCategory=Unit"));
        });

    public static int Main() => Execute<Build>(x => x.RunAllTests);
}