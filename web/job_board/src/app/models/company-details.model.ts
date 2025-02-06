export class CompanyDetails {
  constructor(
    public CompanyId: string,
    public Name: string,
  ) {}

  public static from(obj: CompanyDetails) {
    return new CompanyDetails(obj.CompanyId, obj.Name);
  }
}

export class OfficeLocation {
  constructor(
    public City: string,
    public Country: string,
  ) {}

  public static from(obj: OfficeLocation) {
    return new OfficeLocation(obj.City, obj.Country);
  }
}
