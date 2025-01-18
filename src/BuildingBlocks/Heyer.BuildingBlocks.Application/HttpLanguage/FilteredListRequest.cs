namespace Heyer.BuildingBlocks.Application.HttpLanguage;

public record FilteredListRequest(int Page, int PageSize)
{
    public int PageIx => Page - 1;
}