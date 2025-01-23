import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'hDate',
})
export class HDatePipe implements PipeTransform {
  constructor(private datePipe: DatePipe) {}

  transform(value: Date | string | null | undefined): string {
    if (value === null || value === undefined) {
      return '';
    }

    const formatted = this.datePipe.transform(value, 'yyyy-MM-dd')!;

    return formatted ?? '';
  }
}
