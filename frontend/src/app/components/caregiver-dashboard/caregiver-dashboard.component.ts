import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { CaregiverService } from '../../core/services/caregiver.service';
import { AppointmentService } from '../../core/services/appointment.service';
import { CarePlanService } from '../../core/services/care-plan.service';
import { Appointment } from '../../models/appointment.model';
import { CarePlan, CreateCarePlanRequest } from '../../models/care-plan.model';
import { Caregiver } from '../../models/caregiver.model';

@Component({
  selector: 'app-caregiver-dashboard',
  templateUrl: './caregiver-dashboard.component.html'
})
export class CaregiverDashboardComponent implements OnInit {
  activeTab: 'visits' | 'carePlans' = 'visits';

  caregiverProfile?: Caregiver;
  appointments: Appointment[] = [];
  carePlans: CarePlan[] = [];

  newCarePlan: Partial<CreateCarePlanRequest> = {};
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private caregiverService: CaregiverService,
    private appointmentService: AppointmentService,
    private carePlanService: CarePlanService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.getCurrentUser()?.userId;
    if (!userId) return;

    this.caregiverService.getAll().subscribe(all => {
      this.caregiverProfile = all.find(c => c.userId === userId);
      if (this.caregiverProfile) {
        this.appointmentService.getByCaregiver(this.caregiverProfile.caregiverId)
          .subscribe(data => (this.appointments = data));
        this.carePlanService.getAll().subscribe(data => {
          this.carePlans = data.filter(cp => cp.caregiverId === this.caregiverProfile!.caregiverId);
        });
      }
    });
  }

  setTab(tab: 'visits' | 'carePlans'): void {
    this.activeTab = tab;
  }

  createCarePlan(): void {
    if (!this.caregiverProfile) return;
    this.errorMessage = '';
    this.successMessage = '';

    const payload: CreateCarePlanRequest = {
      patientId: this.newCarePlan.patientId!,
      caregiverId: this.caregiverProfile.caregiverId,
      title: this.newCarePlan.title || '',
      description: this.newCarePlan.description || '',
      medications: this.newCarePlan.medications || '',
      goals: this.newCarePlan.goals || '',
      startDate: this.newCarePlan.startDate || new Date().toISOString().substring(0, 10),
      endDate: this.newCarePlan.endDate
    };

    this.carePlanService.create(payload).subscribe({
      next: (created) => {
        this.carePlans = [created, ...this.carePlans];
        this.successMessage = 'Care plan created successfully.';
        this.newCarePlan = {};
      },
      error: () => (this.errorMessage = 'Could not create care plan.')
    });
  }
}
