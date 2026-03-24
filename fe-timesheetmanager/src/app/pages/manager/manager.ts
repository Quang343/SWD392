import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TimesheetService, Timesheet } from '../../services/timesheet.service';

@Component({
  selector: 'app-manager',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './manager.html',
  styleUrls: ['./manager.css']
})
export class Manager implements OnInit {

  timesheets: Timesheet[] = [];
  isProcessing = false;
  loading = false;
  errorMessage: string | null = null;

  username: string = 'Manager';
  statusMessage: string = '';
  isError: boolean = false;

  pendingCount: number = 0;
  approvedCount: number = 0;
processingIds: number[] = [];
  // ===== NEW =====
  selectedTimesheet: Timesheet | null = null;

  showModal = false;
  rejectComment = '';
  currentRejectId: number | null = null;

  constructor(
    private timesheetService: TimesheetService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  trackById(index: number, ts: Timesheet): number {
    return ts.timesheetId;
  }

  getStatusClass(status: string) {
    return {
      'bg-yellow-100 text-yellow-800': status === 'Pending',
      'bg-green-100 text-green-800': status === 'Approved',
      'bg-red-100 text-red-800': status === 'Rejected'
    };
  }

  loadData() {
    this.loading = true;

    this.timesheetService.getAll().subscribe({
      next: (res) => {
        this.timesheets = Array.isArray(res) ? res : [];

        this.pendingCount = this.timesheets.filter(t => t.status === 'Pending').length;
        this.approvedCount = this.timesheets.filter(t => t.status === 'Approved').length;

        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Không thể tải dữ liệu';
        this.loading = false;
      }
    });
  }

  // ===== EXPAND =====
  toggleRow(ts: Timesheet) {
    if (this.selectedTimesheet?.timesheetId === ts.timesheetId) {
      this.selectedTimesheet = null;
    } else {
      this.selectedTimesheet = ts;
    }
  }

  // ===== EDIT =====
  openEdit(ts: Timesheet) {
    this.selectedTimesheet = ts;
    this.currentRejectId = ts.timesheetId;
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
    this.rejectComment = '';
  }

 approve(ts: Timesheet) {

  if (this.isProcessing) return;

  this.isProcessing = true;

  this.timesheetService.approve(ts.timesheetId, 'Approved by Manager')
    .subscribe({
      next: () => {

        this.statusMessage = 'Đã phê duyệt!';
        this.isError = false;

        // reload lại dữ liệu
        window.location.reload();

        // hoặc reload full page (mạnh hơn)
        // window.location.reload();

      },
      error: () => {
        this.statusMessage = 'Phê duyệt thất bại!';
        this.isError = true;
        this.isProcessing = false;
      },
      complete: () => {
        this.isProcessing = false;
      }
    });
}

  confirmReject() {

  if (!this.currentRejectId || this.isProcessing) return;

  this.isProcessing = true;

  this.timesheetService.reject(this.currentRejectId, this.rejectComment)
    .subscribe({
      next: () => {

        this.statusMessage = 'Đã từ chối!';
        this.isError = false;

        this.closeModal();

        window.location.reload();

        // hoặc:
        // window.location.reload();

      },
      error: () => {
        this.statusMessage = 'Reject failed!';
        this.isError = true;
        this.isProcessing = false;
      },
      complete: () => {
        this.isProcessing = false;
      }
    });
}

  refreshData() {
    this.loadData();
  }

  logout() {
    localStorage.clear();
    window.location.href = '/login';
  }
}