export class JobOfferListItem {
  constructor(
    public id: string,
    public offerSummary: string,
    public publishedAt: Date,
    public publishedUntil: Date,
    public actions: string
  ) {}
}
