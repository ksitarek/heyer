import { Component, input, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HlmInputDirective } from '@spartan-ng/ui-input-helm';
import { HlmLabelDirective } from '@spartan-ng/ui-label-helm';
import { debounceTime, distinct, filter, Observable, Subscription, switchMap, tap } from 'rxjs';
import { JobOfferLocationService } from './joboffer-location.service';

@Component({
  selector: 'h-location-form',
  imports: [ReactiveFormsModule, HlmLabelDirective, HlmInputDirective],
  templateUrl: './location-form.component.html',
  styleUrl: './location-form.component.scss',
})
export class LocationFormComponent implements OnInit, OnDestroy {
  readonly form = input.required<FormGroup>();

  private locationChangedSubscription!: Subscription;

  constructor(private jobOfferLocationService: JobOfferLocationService) {}

  public ngOnInit(): void {
    const idControl = this.form().get('id');
    const location = this.form().get('location');

    const location$ = location?.valueChanges as Observable<{ city: string; country: string }>;

    this.locationChangedSubscription = location$
      .pipe(
        debounceTime(200),
        distinct(),

        filter(() => location?.dirty ?? false),
        filter(() => location?.valid ?? false),

        switchMap(({ city, country }) =>
          this.jobOfferLocationService.setOfficeLocation(idControl?.value as string, city, country),
        ),

        tap(() => location?.markAsPristine()),
      )
      .subscribe();
  }

  public ngOnDestroy(): void {
    this.locationChangedSubscription.unsubscribe();
  }
}
