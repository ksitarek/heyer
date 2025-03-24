import { KeyValue } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NgIcon } from '@ng-icons/core';
import { HlmH1Directive } from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'h-not-found',
  imports: [NgIcon, HlmH1Directive, RouterModule],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss',
})
export class NotFoundComponent implements OnInit {
  private notFoundContents: KeyValue<string, string>[] = [
    { key: 'How did you end up here?', value: 'No, seriously, there is nothing here.' },
    { key: "Lookin' for a job?", value: "Well there ain't any." },
  ];

  public content: KeyValue<string, string> | null = null;

  public ngOnInit(): void {
    this.content = this.notFoundContents[Math.floor(Math.random() * this.notFoundContents.length)];
  }
}
