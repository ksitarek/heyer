using Nuke.Common;
using Nuke.Common.Tools.DotNet;

public partial class Build
{
    Target DotnetBuild => _ => _
        .DependsOn(DotnetRestore)
        .Executes(() =>
        {
            DotNetTasks.DotNetBuild(t => t
                                        .SetConfiguration(_configuration)
                                        .SetProjectFile(_solution)
                                        .SetNoRestore(true));
        });

    Target DotnetClean => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetClean(t => t
                                        .SetConfiguration(_configuration)
                                        .SetProject(_solution));
        });

    Target DotnetRestore => _ => _
        .DependsOn(DotnetClean)
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(t => t.SetProjectFile(_solution));
        });

    Target RunAllTests => _ => _
        .DependsOn(RunUnitTests, RunIntegrationTests)
        .Executes(() =>
        {
        });

    Target RunIntegrationTests => _ => _
        .DependsOn(DotnetBuild)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(_configuration)
                                       .SetProjectFile(_solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true)
                                       .SetFilter("TestCategory=Integration"));
        });

    Target RunUnitTests => _ => _
        .DependsOn(DotnetBuild)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(t => t
                                       .SetConfiguration(_configuration)
                                       .SetProjectFile(_solution)
                                       .SetNoRestore(true)
                                       .SetNoBuild(true)
                                       .SetFilter("TestCategory=Unit"));
        });
}