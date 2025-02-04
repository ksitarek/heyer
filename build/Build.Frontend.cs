using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;

public partial class Build
{
    Target NpmInstall => _ => _
        .Executes(() =>
        {
            NpmTasks.NpmInstall(t => t.SetProcessWorkingDirectory(JobBoardWebPath));
            NpmTasks.NpmInstall(t => t.SetProcessWorkingDirectory(BackofficeWebPath));
        });
}