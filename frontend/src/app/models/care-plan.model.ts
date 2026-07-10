export interface CarePlan {
  carePlanId: number;
  patientId: number;
  caregiverId: number;
  title: string;
  description: string;
  medications: string;
  goals: string;
  startDate: string;
  endDate?: string;
  status: 'Active' | 'Completed';
  createdAt: string;
  patientName?: string;
  caregiverName?: string;
}

export interface CreateCarePlanRequest {
  patientId: number;
  caregiverId: number;
  title: string;
  description: string;
  medications: string;
  goals: string;
  startDate: string;
  endDate?: string;
}
