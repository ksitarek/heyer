namespace Heyer.BuildingBlocks.Application.Authorization;

public interface IUserDataProvider
{
    Guid CompanyId { get; }
    string CompanyName { get; }
    Guid UserId { get; }
}