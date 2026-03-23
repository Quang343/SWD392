import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApprovalService {

  private api = 'https://localhost:7156/api/approval';

  constructor(private http: HttpClient) {}

  approve(data: any) {
    return this.http.post(`${this.api}/approve`, data);
  }

  reject(data: any) {
    return this.http.post(`${this.api}/reject`, data);
  }
}