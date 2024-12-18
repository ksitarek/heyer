namespace Heyer.BuildingBlocks.Application.Authorization;

public interface IUserDataProvider
{
    Guid UserId { get; }
    Guid CompanyId { get; }
    string CompanyName { get; }
}