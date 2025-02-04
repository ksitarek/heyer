using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.Docker;
using Serilog;

// ReSharper disable AllUnderscoreLocalParameterName

partial class Build : NukeBuild
{
    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration _configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution("Heyer.sln")] readonly Solution _solution;

    public Build() =>
        DockerTasks.DockerLogger = (_, m) =>
            Log.Information(m);

    public static int Main() => Execute<Build>(x => x.RunAllTests);
}