import { Component, OnInit } from '@angular/core';
import { Patient } from '../../models/patient.model';
import { Caregiver } from '../../models/caregiver.model';
import { Appointment, CreateAppointmentRequest } from '../../models/appointment.model';
import { PatientService } from '../../core/services/patient.service';
import { CaregiverService } from '../../core/services/caregiver.service';
import { AppointmentService } from '../../core/services/appointment.service';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html'
})
export class AdminDashboardComponent implements OnInit {
  activeTab: 'overview' | 'patients' | 'caregivers' | 'appointments' = 'overview';

  patients: Patient[] = [];
  caregivers: Caregiver[] = [];
  appointments: Appointment[] = [];

  newAppointment: CreateAppointmentRequest = {
    patientId: 0,
    caregiverId: 0,
    scheduledDate: '',
    timeSlot: ''
  };

  errorMessage = '';
  successMessage = '';

  constructor(
    private patientService: PatientService,
    private caregiverService: CaregiverService,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.patientService.getAll().subscribe(data => (this.patients = data));
    this.caregiverService.getAll().subscribe(data => (this.caregivers = data));
    this.appointmentService.getAll().subscribe(data => (this.appointments = data));
  }

  setTab(tab: 'overview' | 'patients' | 'caregivers' | 'appointments'): void {
    this.activeTab = tab;
  }

  bookAppointment(): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.appointmentService.create(this.newAppointment).subscribe({
      next: () => {
        this.successMessage = 'Appointment booked successfully.';
        this.appointmentService.getAll().subscribe(data => (this.appointments = data));
        this.newAppointment = { patientId: 0, caregiverId: 0, scheduledDate: '', timeSlot: '' };
      },
      error: (err) => {
        this.errorMessage = err?.error?.message || 'Could not book this appointment.';
      }
    });
  }

  deletePatient(id: number): void {
    this.patientService.delete(id).subscribe(() => {
      this.patients = this.patients.filter(p => p.patientId !== id);
    });
  }

  deleteCaregiver(id: number): void {
    this.caregiverService.delete(id).subscribe(() => {
      this.caregivers = this.caregivers.filter(c => c.caregiverId !== id);
    });
  }
}
