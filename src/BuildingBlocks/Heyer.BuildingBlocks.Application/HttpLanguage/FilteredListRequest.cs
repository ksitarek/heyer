namespace Heyer.BuildingBlocks.Application.HttpLanguage;

public record FilteredListRequest(int Page, int PageSize, SortRequest? Sort)
{
    public int PageIx => Page - 1;
}