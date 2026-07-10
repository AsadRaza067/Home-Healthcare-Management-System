import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Caregiver } from '../../models/caregiver.model';

@Injectable({ providedIn: 'root' })
export class CaregiverService {
  private apiUrl = `${environment.apiBaseUrl}/caregivers`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Caregiver[]> {
    return this.http.get<Caregiver[]>(this.apiUrl);
  }

  getById(id: number): Observable<Caregiver> {
    return this.http.get<Caregiver>(`${this.apiUrl}/${id}`);
  }

  update(id: number, caregiver: Caregiver): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, caregiver);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
