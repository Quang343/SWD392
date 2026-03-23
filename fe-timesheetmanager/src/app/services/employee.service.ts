import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface EmployeeProfile {
  employeeId: number;
  fullName: string;
  email: string;
  department: string;
  position: string;
  status: string;
  username: string;
}

export interface UpdateProfileRequest {
  fullName: string;
  department: string;
  position: string;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  private apiUrl = 'https://localhost:7156/api/employee';

  constructor(private http: HttpClient) {}

  getProfile(): Observable<EmployeeProfile> {
    return this.http.get<EmployeeProfile>(`${this.apiUrl}/profile`);
  }

  updateProfile(data: UpdateProfileRequest): Observable<any> {
    return this.http.put(`${this.apiUrl}/profile`, data);
  }
}
