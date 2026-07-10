export interface AuthResponse {
  token: string;
  fullName: string;
  email: string;
  role: 'Admin' | 'Caregiver' | 'Patient';
  userId: number;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  role: 'Admin' | 'Caregiver' | 'Patient';
  phone: string;
  specialization?: string;
  address?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
