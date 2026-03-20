import { Component } from '@angular/core';
import { ApprovalService } from '../../services/approval.service';

@Component({
  selector: 'app-manager',
  standalone: true,
  template: `
    <h2>Manager Page</h2>

    <button (click)="approve()">Approve</button>
    <button (click)="reject()">Reject</button>
  `
})
export class Manager {

  constructor(private service: ApprovalService) {}

  approve() {
    this.service.approve({
      timesheetId: 1,
      comment: 'OK'
    }).subscribe(res => console.log(res));
  }

  reject() {
    this.service.reject({
      timesheetId: 1,
      comment: 'Not OK'
    }).subscribe(res => console.log(res));
  }
}