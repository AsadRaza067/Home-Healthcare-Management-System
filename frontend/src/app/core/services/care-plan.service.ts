import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CarePlan, CreateCarePlanRequest } from '../../models/care-plan.model';

@Injectable({ providedIn: 'root' })
export class CarePlanService {
  private apiUrl = `${environment.apiBaseUrl}/careplans`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<CarePlan[]> {
    return this.http.get<CarePlan[]>(this.apiUrl);
  }

  getByPatient(patientId: number): Observable<CarePlan[]> {
    return this.http.get<CarePlan[]>(`${this.apiUrl}/patient/${patientId}`);
  }

  create(payload: CreateCarePlanRequest): Observable<CarePlan> {
    return this.http.post<CarePlan>(this.apiUrl, payload);
  }

  update(id: number, carePlan: CarePlan): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, carePlan);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
