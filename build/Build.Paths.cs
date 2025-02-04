using Nuke.Common.IO;

public partial class Build
{
    AbsolutePath ApiPath => RootDirectory / "src/API/Heyer.API";
    AbsolutePath BackofficeWebPath => WebPath / "backoffice";
    AbsolutePath DBMigratorPath => RootDirectory / "src/Meta/Heyer.Meta.DbMigrator";
    AbsolutePath JobBoardWebPath => WebPath / "job_board";
    AbsolutePath StorageApiPath => RootDirectory / "src/API/Heyer.Storage.API";
    AbsolutePath WebPath => RootDirectory / "web";
}