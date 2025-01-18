export class ListResponse<T> {
  constructor(
    public pageSize: number,
    public totalCount: number,
    public items: T[]
  ) {}

  public static from<T>(response: any): ListResponse<T> {
    const x = new ListResponse<T>(
      response.PageSize,
      response.TotalCount,
      response.Items
    );

    console.log(x);

    return x;
  }
}
