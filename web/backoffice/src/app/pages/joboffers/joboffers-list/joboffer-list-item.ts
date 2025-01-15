export class JobOfferListItem {
  constructor(
    public id: string,
    public offerSummary: string,
    public publishedAt: Date,
    public publishedUntil: Date,
    public actions: string
  ) {}

  public static from(item: any): JobOfferListItem {
    return new JobOfferListItem(
      item.id,
      item.offerSummary,
      new Date(item.publishedAt),
      new Date(item.publishedUntil),
      item.actions
    );
  }
}
