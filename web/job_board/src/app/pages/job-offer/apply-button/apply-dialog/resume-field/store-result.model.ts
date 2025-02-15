export class StoreResult {
  constructor(public fileHandle: string) {}

  public static from(obj: StoreResult) {
    return new StoreResult(obj.fileHandle);
  }
}
