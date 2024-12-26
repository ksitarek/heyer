namespace Heyer.BuildingBlocks.Application.Authorization;

public class ValueUserDataProvider : IUserDataProvider
{
    public Guid CompanyId { get; private set; }
    public string CompanyName { get; private set; } = "";
    public Guid UserId { get; private set; }

    public void SetExecutionContext(Guid userId, Guid companyId, string companyName)
    {
        CompanyId = companyId;
        CompanyName = companyName;
        UserId = userId;
    }
}