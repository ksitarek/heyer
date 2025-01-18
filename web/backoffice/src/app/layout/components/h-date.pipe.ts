import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hDate',
})
export class HDatePipe implements PipeTransform {
  constructor(private datePipe: DatePipe) {}

  transform(value: any): string {
    return this.datePipe.transform(value, 'yyyy-MM-dd')!;
  }
}
