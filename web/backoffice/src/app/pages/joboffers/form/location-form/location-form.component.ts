import { Component, input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import {
  combineLatest,
  debounceTime,
  distinct,
  filter,
  map,
  Observable,
  startWith,
  Subscription,
  switchMap,
} from 'rxjs';
import { JobOfferLocationService } from './joboffer-location.service';

@Component({
  selector: 'h-location-form',
  imports: [ReactiveFormsModule, HlmLabelDirective, HlmInputDirective],
  templateUrl: './location-form.component.html',
  styleUrl: './location-form.component.scss',
})
export class LocationFormComponent implements OnInit, OnDestroy {
  readonly form = input.required<FormGroup>();

  private locationChanged$!: Subscription;

  constructor(private JobOfferLocationService: JobOfferLocationService) {}

  public ngOnInit(): void {
    const idControl = this.form().get('id');
    const cityControl = this.form().get('location.city');
    const countryControl = this.form().get('location.country');

    const cityControl$ = cityControl?.valueChanges as Observable<string>;
    const countryControl$ = countryControl?.valueChanges as Observable<string>;

    this.locationChanged$ = combineLatest([
      cityControl$.pipe(startWith(cityControl?.value as string)),
      countryControl$.pipe(startWith(countryControl?.value as string)),
    ])
      .pipe(
        debounceTime(500),
        distinct(),

        map(([city, country]) => ({ city, country })),

        filter(() => (cityControl?.dirty ?? false) || (countryControl?.dirty ?? false)),
        filter(({ city, country }) => city.length > 0 && country.length > 0),

        switchMap(({ city, country }) =>
          this.JobOfferLocationService.setOfficeLocation(idControl?.value as string, city, country),
        ),
      )
      .subscribe();
  }

  public ngOnDestroy(): void {
    this.locationChanged$.unsubscribe();
  }
}
