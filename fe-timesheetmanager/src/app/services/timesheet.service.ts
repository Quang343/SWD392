import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

/* ================== EXISTING MODELS (GIỮ NGUYÊN) ================== */

export interface MyTask {
  taskItemId: number;
  taskName: string;
  projectName: string;
}

export interface MyTimesheetEntry {
  timesheetEntryId?: number;
  taskItemId: number;
  workDate: string;
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

/* ================== NEW MODEL (KHÔNG ẢNH HƯỞNG) ================== */

export interface Timesheet {
  timesheetId: number;
  employeeName: string;
  status: string;
  entries: TimesheetEntry[];
}
export interface TimesheetEntry {
  workDate: string;
  hoursWorked: number;
  taskName: string;
  // nếu backend trả thêm field khác thì bổ sung ở đây
}
/* ================== SERVICE ================== */

@Injectable({
  providedIn: 'root'
})
export class TimesheetService {

  private apiUrl = 'https://localhost:7156/api/timesheet';
  private approvalUrl = 'https://localhost:7156/api/approval';

  constructor(private http: HttpClient) {}

  /* ===== FIX NHẸ: thêm header nhưng KHÔNG ảnh hưởng code cũ ===== */
  private getHeaders() {
    const token = localStorage.getItem('token');

    if (!token) return {}; // ❗ nếu chưa login vẫn chạy như cũ

    return {
      headers: new HttpHeaders({
        Authorization: 'Bearer ' + token
      })
    };
  }

  /* ================== EXISTING APIs (GIỮ NGUYÊN) ================== */

  getMyTasks(): Observable<MyTask[]> {
    return this.http.get<MyTask[]>(`${this.apiUrl}/my-tasks`, this.getHeaders());
  }

  getAvailableTasks(): Observable<MyTask[]> {
    return this.http.get<MyTask[]>(`${this.apiUrl}/available-tasks`, this.getHeaders());
  }

  getMyTimesheet(startDate: string): Observable<MyTimesheet | null> {
    return this.http.get<MyTimesheet | null>(
      `${this.apiUrl}/my-timesheet?startDate=${startDate}`,
      this.getHeaders()
    );
  }

  saveTimesheet(data: SaveTimesheetRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/save`, data, this.getHeaders());
  }

  /* ================== NEW APIs (CHO MANAGER) ================== */

  getAll(): Observable<Timesheet[]> {
    return this.http.get<Timesheet[]>(`${this.apiUrl}`, this.getHeaders());
  }

  approve(timesheetId: number, comment: string) {
    return this.http.post(`${this.approvalUrl}/approve`, {
      timesheetId,
      comment
    }, this.getHeaders());
  }

  reject(timesheetId: number, comment: string) {
    return this.http.post(`${this.approvalUrl}/reject`, {
      timesheetId,
      comment
    }, this.getHeaders());
  }
}