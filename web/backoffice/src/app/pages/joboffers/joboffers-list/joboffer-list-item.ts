export class JobOfferListItem {
  constructor(
    public id: string,
    public offerSummary: string,
    public publishedAt: Date | null,
    public publishedUntil: Date | null,
    public actions: string
  ) {}

  public static from(item: any): JobOfferListItem {
    return new JobOfferListItem(
      item.Id,
      item.OfferSummary,
      item.PublishedAt == null ? null : new Date(item.PublishedAt),
      item.PublishedUntil == null ? null : new Date(item.PublishedUntil),
      item.Actions
    );
  }
}
