export class NewCandidateApplication {
  public constructor(
    public PublishedJobOfferId: string,
    public FirstName: string,
    public LastName: string,
    public Email: string,
    public ResumeKey: string,
    public IncludeInCandidatePool: boolean,
  ) {}
}
