import { NgIf } from '@angular/common';
import {
  Component,
  computed,
  effect,
  input,
  output,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
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
import { NgIcon } from '@ng-icons/core';

@Component({
  selector: 'h-pagination',
  imports: [
    NgIf,
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
  public totalCount = input(0);
  public pageChanged = output<number>();
  public pageSizes = input([20, 50, 100]);

  protected readonly currentPage = signal(1);
  protected readonly pageSize = signal(20);
  protected readonly pageIx = computed(() => this.currentPage() - 1);
  protected readonly from = computed(() => this.pageSize() * this.pageIx() + 1);
  protected readonly to = computed(() =>
    Math.min(this.currentPage() * this.pageSize(), this.totalCount()),
  );

  protected pageChangedEffect = effect(() => {
    const page = this.pageIx();
    this.pageChanged.emit(page);
  });

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
