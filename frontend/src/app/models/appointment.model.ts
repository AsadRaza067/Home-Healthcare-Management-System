export interface Appointment {
  appointmentId: number;
  patientId: number;
  caregiverId: number;
  scheduledDate: string;
  timeSlot: string;
  status: 'Scheduled' | 'Completed' | 'Cancelled';
  visitNotes: string;
  createdAt: string;
  patientName?: string;
  caregiverName?: string;
}

export interface CreateAppointmentRequest {
  patientId: number;
  caregiverId: number;
  scheduledDate: string;
  timeSlot: string;
}
