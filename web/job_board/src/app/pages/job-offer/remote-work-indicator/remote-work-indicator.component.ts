import { Component, computed, input } from '@angular/core';
import { NgIcon } from '@ng-icons/core';
import { RemoteWork } from '../../../models/remote-work.model';

@Component({
  selector: 'h-remote-work-indicator',
  imports: [NgIcon],
  templateUrl: './remote-work-indicator.component.html',
  styleUrl: './remote-work-indicator.component.scss',
})
export class RemoteWorkIndicatorComponent {
  public readonly remoteWork = input.required<RemoteWork>();

  private readonly isRemote = computed(() => this.remoteWork() === RemoteWork.Yes);

  private readonly isHybrid = computed(() => this.remoteWork() === RemoteWork.Hybrid);

  private readonly isOnSite = computed(() => this.remoteWork() === RemoteWork.No);

  public readonly icon = computed(() => {
    if (this.isRemote()) {
      return 'lucideHeadset';
    } else if (this.isHybrid()) {
      return 'lucideArrowLeftRight';
    } else {
      return 'lucideBuilding';
    }
  });

  public readonly label = computed(() => {
    if (this.isRemote()) {
      return 'Remote';
    } else if (this.isHybrid()) {
      return 'Hybrid';
    } else {
      return 'On site';
    }
  });
}
