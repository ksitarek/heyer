import { Component, input } from '@angular/core';
import { HlmCardContentDirective, HlmCardDirective } from '@spartan-ng/ui-card-helm';
import { ContractDetails } from '../../../models/contract-details.model';
import { ContractDetailsComponent } from './contract-details/contract-details.component';

@Component({
  selector: 'h-contracts-details',
  imports: [HlmCardDirective, HlmCardContentDirective, ContractDetailsComponent],
  templateUrl: './contracts-details.component.html',
  styleUrl: './contracts-details.component.scss',
})
export class ContractsDetailsComponent {
  public readonly contractsDetails = input.required<ContractDetails[]>();
}
