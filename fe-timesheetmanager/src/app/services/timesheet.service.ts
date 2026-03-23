import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface MyTask {
  taskItemId: number;
  taskName: string;
  projectName: string;
}

export interface MyTimesheetEntry {
  timesheetEntryId?: number;
  taskItemId: number;
  workDate: string; // ISO format YYYY-MM-DD
  hoursWorked: number;
  note: string;
}

export interface MyTimesheet {
  timesheetId: number;
  weekStartDate: string;
  weekEndDate: string;
  status: string;
  entries: MyTimesheetEntry[];
}

export interface SaveTimesheetRequest {
  weekStartDate: string;
  weekEndDate: string;
  isSubmit: boolean;
  entries: MyTimesheetEntry[];
}

@Injectable({
  providedIn: 'root'
})
export class TimesheetService {

  private apiUrl = 'https://localhost:7156/api/timesheet';

  constructor(private http: HttpClient) {}

  getMyTasks(): Observable<MyTask[]> {
    return this.http.get<MyTask[]>(`${this.apiUrl}/my-tasks`);
  }

  getAvailableTasks(): Observable<MyTask[]> {
    return this.http.get<MyTask[]>(`${this.apiUrl}/available-tasks`);
  }

  getMyTimesheet(startDate: string): Observable<MyTimesheet | null> {
    return this.http.get<MyTimesheet | null>(`${this.apiUrl}/my-timesheet?startDate=${startDate}`);
  }

  saveTimesheet(data: SaveTimesheetRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/save`, data);
  }
}
