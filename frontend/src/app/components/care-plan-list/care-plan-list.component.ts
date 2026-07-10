import { Component, Input } from '@angular/core';
import { CarePlan } from '../../models/care-plan.model';

@Component({
  selector: 'app-care-plan-list',
  templateUrl: './care-plan-list.component.html'
})
export class CarePlanListComponent {
  @Input() carePlans: CarePlan[] = [];
}
