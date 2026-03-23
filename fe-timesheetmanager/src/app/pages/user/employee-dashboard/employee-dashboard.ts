import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { TimesheetService, MyTask, MyTimesheet, SaveTimesheetRequest, MyTimesheetEntry } from '../../../services/timesheet.service';

interface DailyEntry {
  date: Date;
  hours: number;
}

interface TaskRow {
  taskItemId: number;
  taskName: string;
  projectName: string;
  entries: DailyEntry[];
}

@Component({
  selector: 'app-employee-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './employee-dashboard.html',
  styleUrl: './employee-dashboard.css'
})
export class EmployeeDashboard implements OnInit {
  username: string = '';

  tasks: MyTask[] = [];
  timesheet: MyTimesheet | null = null;

  currentWeekStart!: Date;
  weekDays: Date[] = [];

  taskRows: TaskRow[] = [];

  statusMessage: string = '';
  isError: boolean = false;

  constructor(
    private authService: AuthService,
    private timesheetService: TimesheetService,
    private router: Router
  ) {
    this.username = localStorage.getItem('username') || 'Employee';
    // Mặc định, theo seed data, ta chọn tuần 2025-03-17 làm ví dụ để có dữ liệu
    this.currentWeekStart = new Date(2025, 2, 17); // Tháng 2 = March trong TS Date
  }

  ngOnInit() {
    this.loadWeekData();
  }

  generateWeekDays(start: Date) {
    this.weekDays = [];
    for (let i = 0; i < 7; i++) {
      let d = new Date(start);
      d.setDate(d.getDate() + i);
      this.weekDays.push(d);
    }
  }

  formatDate(d: Date): string {
    const year = d.getFullYear();
    const month = (d.getMonth() + 1).toString().padStart(2, '0');
    const day = d.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  loadWeekData() {
    this.generateWeekDays(this.currentWeekStart);

    // 1. Get tasks (Lấy tất cả available tasks để form luôn hiển thị sẵn)
    this.timesheetService.getAvailableTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
        this.availableTasks = tasks;
        this.buildTaskRows();

        // 2. Get timesheet
        this.timesheetService.getMyTimesheet(this.formatDate(this.currentWeekStart)).subscribe({
          next: (ts) => {
            this.timesheet = ts;
            this.mapTimesheetToRows();
          },
          error: (err) => {
            console.error('Error loading timesheet:', err);
          }
        });
      },
      error: (err) => {
        console.error('Error loading tasks:', err);
        this.statusMessage = 'Không thể tải danh sách task. Vui lòng kiểm tra kết nối Backend.';
        this.isError = true;
      }
    });
  }

  buildTaskRows() {
    this.taskRows = this.tasks.map(t => ({
      taskItemId: t.taskItemId,
      taskName: t.taskName,
      projectName: t.projectName,
      entries: this.weekDays.map(d => ({
        date: new Date(d),
        hours: 0
      }))
    }));
  }

  mapTimesheetToRows() {
    if (!this.timesheet) return;

    this.timesheet.entries.forEach(entry => {
      // Nếu task từ timesheet chưa có trong (MyTask / Assigned Tasks), ta tự push vào taskRows để hiển thị
      let row = this.taskRows.find(r => r.taskItemId === entry.taskItemId);
      if (!row) {
        const tInfo = this.availableTasks.find(t => t.taskItemId === entry.taskItemId);
        row = {
          taskItemId: entry.taskItemId,
          taskName: tInfo ? tInfo.taskName : 'Task ID: ' + entry.taskItemId,
          projectName: tInfo ? tInfo.projectName : 'Unknown Project',
          entries: this.weekDays.map(d => ({ date: new Date(d), hours: 0 }))
        };
        this.taskRows.push(row);
      }

      const cell = row.entries.find(e => this.formatDate(e.date) === entry.workDate.split('T')[0]);
      if (cell) {
        cell.hours = entry.hoursWorked;
      }
    });
  }

  showAddTaskModal: boolean = false;
  availableTasks: MyTask[] = [];
  selectedTaskId: number | null = null;

  openTaskModal() {
    this.timesheetService.getAvailableTasks().subscribe(res => {
      this.availableTasks = res;
      this.showAddTaskModal = true;
    });
  }

  closeTaskModal() {
    this.showAddTaskModal = false;
    this.selectedTaskId = null;
  }

  addTaskToTimesheet() {
    if (!this.selectedTaskId) return;

    const exists = this.taskRows.find(r => r.taskItemId == this.selectedTaskId);
    if (!exists) {
      const tInfo = this.availableTasks.find(t => t.taskItemId == this.selectedTaskId);
      if (tInfo) {
        this.taskRows.push({
          taskItemId: tInfo.taskItemId,
          taskName: tInfo.taskName,
          projectName: tInfo.projectName,
          entries: this.weekDays.map(d => ({ date: new Date(d), hours: 0 }))
        });
      }
    }

    this.closeTaskModal();
  }

  prevWeek() {
    this.currentWeekStart.setDate(this.currentWeekStart.getDate() - 7);
    this.loadWeekData();
  }

  nextWeek() {
    this.currentWeekStart.setDate(this.currentWeekStart.getDate() + 7);
    this.loadWeekData();
  }

  // --- Tính Toán ---
  getRowTotal(row: TaskRow): number {
    return row.entries.reduce((sum, e) => sum + (e.hours || 0), 0);
  }

  getDailyTotal(dayIndex: number): number {
    return this.taskRows.reduce((sum, row) => sum + (row.entries[dayIndex]?.hours || 0), 0);
  }

  getWeeklyTotal(): number {
    let total = 0;
    for (let i = 0; i < 7; i++) {
      total += this.getDailyTotal(i);
    }
    return total;
  }

  getOvertimeTotal(): number {
    let oTotal = 0;
    this.taskRows.forEach(row => {
      row.entries.forEach(e => {
        if (e.hours > 8) {
          oTotal += (e.hours - 8);
        }
      });
    });
    return oTotal;
  }

  // --- Lưu & Submit ---
  prepareRequest(isSubmit: boolean): SaveTimesheetRequest {
    const request: SaveTimesheetRequest = {
      weekStartDate: this.formatDate(this.currentWeekStart),
      weekEndDate: this.formatDate(this.weekDays[6]),
      isSubmit: isSubmit,
      entries: []
    };

    this.taskRows.forEach(row => {
      row.entries.forEach(e => {
        if (e.hours > 0) {
          request.entries.push({
            taskItemId: row.taskItemId,
            workDate: this.formatDate(e.date),
            hoursWorked: e.hours,
            note: ''
          });
        }
      });
    });

    return request;
  }

  saveDraft() {
    const req = this.prepareRequest(false);
    this.submitTimesheetData(req);
  }

  submitForApproval() {
    const req = this.prepareRequest(true);
    this.submitTimesheetData(req);
  }

  private submitTimesheetData(req: SaveTimesheetRequest) {
    this.timesheetService.saveTimesheet(req).subscribe({
      next: (res) => {
        this.statusMessage = res.message;
        this.isError = false;
        setTimeout(() => this.statusMessage = '', 3000);
        this.loadWeekData(); // reload
      },
      error: (err) => {
        this.statusMessage = err.error?.message || 'Có lỗi xảy ra!';
        this.isError = true;
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
