import { Component, computed, effect, input, model } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgIcon } from '@ng-icons/core';
import { BrnSelectImports } from '@spartan-ng/brain/select';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import {
  HlmSelectContentDirective,
  HlmSelectImports,
  HlmSelectOptionComponent,
  HlmSelectTriggerComponent,
  HlmSelectValueDirective,
} from '@spartan-ng/ui-select-helm';
import {
  HlmMutedDirective,
  HlmSmallDirective,
} from '@spartan-ng/ui-typography-helm';

@Component({
  selector: 'h-pagination',
  imports: [
    FormsModule,
    BrnSelectImports,
    HlmSelectImports,
    HlmSmallDirective,
    HlmMutedDirective,
    HlmSelectTriggerComponent,
    HlmSelectValueDirective,
    HlmSelectContentDirective,
    HlmSelectOptionComponent,
    HlmButtonDirective,
    NgIcon,
  ],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.scss',
})
export class PaginationComponent {
  public readonly totalCount = input(0);
  public readonly pageSizes = input([10, 50, 100]);

  public readonly currentPage = model<number>(0);
  public readonly pageSize = model<number>(0);

  protected readonly pageIx = computed(() => this.currentPage() - 1);
  protected readonly from = computed(() => this.pageSize() * this.pageIx() + 1);
  protected readonly to = computed(() =>
    Math.min(this.currentPage() * this.pageSize(), this.totalCount()),
  );

  protected pageSizeChangedEffect = effect(() => {
    this.currentPage.set(1);
  });

  protected readonly hasPreviousPage = computed(() => this.pageIx() > 0);
  protected readonly hasNextPage = computed(
    () => this.to() < this.totalCount(),
  );

  protected previousPage(): void {
    if (this.hasPreviousPage()) {
      this.currentPage.set(this.currentPage() - 1);
    }
  }

  protected nextPage(): void {
    if (this.hasNextPage()) {
      this.currentPage.set(this.currentPage() + 1);
    }
  }
}
