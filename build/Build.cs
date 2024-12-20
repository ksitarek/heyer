using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Docker;
using Nuke.Common.Tools.DotNet;
using Serilog;

// ReSharper disable AllUnderscoreLocalParameterName

partial class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    readonly AbsolutePath StorageApiPath = RootDirectory / "src/API/Heyer.Storage.API";

    public Build() =>
        DockerTasks.DockerLogger = (_, m) =>
            Log.Information(m);

    Target Clean => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(t => t
                                        .SetConfiguration(Configuration)
                                        .SetProject(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(t => t
                                        .SetConfiguration(Configuration)
                                        .SetProjectFile(Solution)
                                        .SetNoRestore(true));
        });

    Target IntegrationTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(Configuration)
                                       .SetProjectFile(Solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true)
                                       .SetFilter("TestCategory=Integration"));
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(t => t.SetProjectFile(Solution));
        });

    Target RunAllTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(Configuration)
                                       .SetProjectFile(Solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true));
        });

    Target UnitTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(Configuration)
                                       .SetProjectFile(Solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true)
                                       .SetFilter("TestCategory=Unit"));
        });

    public static int Main() => Execute<Build>(x => x.RunAllTests);
}