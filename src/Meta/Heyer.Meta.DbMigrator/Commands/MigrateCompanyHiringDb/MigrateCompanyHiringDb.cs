using MediatR;

namespace Heyer.Meta.DbMigrator.Commands.MigrateCompanyHiringDb;

internal record MigrateCompanyHiringDb(string CompanyId) : IRequest;