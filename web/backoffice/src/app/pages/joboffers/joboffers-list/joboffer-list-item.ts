export class JobOfferListItem {
  constructor(
    public Id: string,
    public OfferSummary: string,
    public PublishedAt: Date | null,
    public PublishedUntil: Date | null,
    public Actions: string,
  ) {}

  public static from(item: JobOfferListItem): JobOfferListItem {
    return new JobOfferListItem(
      item.Id,
      item.OfferSummary,
      item.PublishedAt == null ? null : new Date(item.PublishedAt),
      item.PublishedUntil == null ? null : new Date(item.PublishedUntil),
      item.Actions,
    );
  }
}
