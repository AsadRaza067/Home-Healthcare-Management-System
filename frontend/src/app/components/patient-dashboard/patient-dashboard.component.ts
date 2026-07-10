import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { PatientService } from '../../core/services/patient.service';
import { CaregiverService } from '../../core/services/caregiver.service';
import { AppointmentService } from '../../core/services/appointment.service';
import { CarePlanService } from '../../core/services/care-plan.service';
import { Patient } from '../../models/patient.model';
import { Caregiver } from '../../models/caregiver.model';
import { Appointment, CreateAppointmentRequest } from '../../models/appointment.model';
import { CarePlan } from '../../models/care-plan.model';

@Component({
  selector: 'app-patient-dashboard',
  templateUrl: './patient-dashboard.component.html'
})
export class PatientDashboardComponent implements OnInit {
  activeTab: 'appointments' | 'carePlans' = 'appointments';

  patientProfile?: Patient;
  caregivers: Caregiver[] = [];
  appointments: Appointment[] = [];
  carePlans: CarePlan[] = [];

  newAppointment: Partial<CreateAppointmentRequest> = {};
  errorMessage = '';
  successMessage = '';

  constructor(
    private authService: AuthService,
    private patientService: PatientService,
    private caregiverService: CaregiverService,
    private appointmentService: AppointmentService,
    private carePlanService: CarePlanService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.getCurrentUser()?.userId;
    if (!userId) return;

    this.patientService.getAll().subscribe(all => {
      this.patientProfile = all.find(p => p.userId === userId);
      if (this.patientProfile) {
        this.appointmentService.getByPatient(this.patientProfile.patientId)
          .subscribe(data => (this.appointments = data));
        this.carePlanService.getByPatient(this.patientProfile.patientId)
          .subscribe(data => (this.carePlans = data));
      }
    });

    this.caregiverService.getAll().subscribe(data => (this.caregivers = data));
  }

  setTab(tab: 'appointments' | 'carePlans'): void {
    this.activeTab = tab;
  }

  bookAppointment(): void {
    if (!this.patientProfile) return;
    this.errorMessage = '';
    this.successMessage = '';

    const payload: CreateAppointmentRequest = {
      patientId: this.patientProfile.patientId,
      caregiverId: this.newAppointment.caregiverId!,
      scheduledDate: this.newAppointment.scheduledDate || '',
      timeSlot: this.newAppointment.timeSlot || ''
    };

    this.appointmentService.create(payload).subscribe({
      next: (created) => {
        this.appointments = [created, ...this.appointments];
        this.successMessage = 'Appointment requested successfully.';
        this.newAppointment = {};
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Could not book this appointment.';
      }
    });
  }
}
