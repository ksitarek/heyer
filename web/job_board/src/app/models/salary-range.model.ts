export class SalaryRange {
  public constructor(
    public IsPublished: boolean,
    public From: number,
    public To: number,
  ) {}

  public static from(obj: SalaryRange) {
    return new SalaryRange(obj.IsPublished, obj.From, obj.To);
  }
}
