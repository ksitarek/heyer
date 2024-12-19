using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

// ReSharper disable AllUnderscoreLocalParameterName

class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

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