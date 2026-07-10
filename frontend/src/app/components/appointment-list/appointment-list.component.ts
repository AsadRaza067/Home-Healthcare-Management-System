import { Component, Input, OnChanges } from '@angular/core';
import { Appointment } from '../../models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';

@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html'
})
export class AppointmentListComponent implements OnChanges {
  @Input() appointments: Appointment[] = [];
  @Input() canManage = false; // Admin/Caregiver can update status

  notesDraft: Record<number, string> = {};

  constructor(private appointmentService: AppointmentService) {}

  ngOnChanges(): void {
    this.appointments.forEach(a => (this.notesDraft[a.appointmentId] = a.visitNotes));
  }

  badgeClass(status: string): string {
    return 'badge badge-' + status.toLowerCase();
  }

  markCompleted(appointment: Appointment): void {
    this.updateStatus(appointment, 'Completed');
  }

  cancelAppointment(appointment: Appointment): void {
    this.updateStatus(appointment, 'Cancelled');
  }

  private updateStatus(appointment: Appointment, status: string): void {
    const notes = this.notesDraft[appointment.appointmentId] ?? appointment.visitNotes;
    this.appointmentService.updateStatus(appointment.appointmentId, status, notes).subscribe({
      next: () => {
        appointment.status = status as Appointment['status'];
        appointment.visitNotes = notes;
      }
    });
  }
}
