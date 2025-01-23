export class ListResponse<T> {
  constructor(
    public PageSize: number,
    public TotalCount: number,
    public Items: T[],
  ) {}

  public static from<T>(response: ListResponse<T>): ListResponse<T> {
    const x = new ListResponse<T>(
      response.PageSize,
      response.TotalCount,
      response.Items,
    );

    console.log(x);

    return x;
  }
}
