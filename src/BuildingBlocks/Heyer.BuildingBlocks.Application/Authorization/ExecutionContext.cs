namespace Heyer.BuildingBlocks.Application.Authorization;

public record ExecutionContext(Guid UserId, Guid CompanyId, string CompanyName);