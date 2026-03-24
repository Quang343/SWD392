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

  viewMode: 'list' | 'detail' = 'list';
  myTimesheets: MyTimesheet[] = [];

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
    // Đã thay đổi khởi tạo date time động ở đây theo log mới của hệ thống thay vì fix cứng seed data
    const today = new Date();
    const day = today.getDay() || 7;
    today.setDate(today.getDate() - day + 1);
    this.currentWeekStart = new Date(today.getFullYear(), today.getMonth(), today.getDate());
  }

  ngOnInit() {
    this.loadTimesheets();
  }

  loadTimesheets() {
    this.timesheetService.getMyTimesheets().subscribe({
      next: (data) => {
        // Sao chép an toàn để Angular nhận biết thay đổi và tránh lỗi sort in-place
        let safeData = Array.isArray(data) ? [...data] : [];
        this.myTimesheets = safeData.sort((a, b) => {
          const dA = a.weekStartDate ? new Date(a.weekStartDate).getTime() : 0;
          const dB = b.weekStartDate ? new Date(b.weekStartDate).getTime() : 0;
          return dB - dA;
        });
        this.viewMode = 'list';
      },
      error: (err) => {
        console.error('Error loading timesheets', err);
        this.statusMessage = err.error?.message || 'Không thể tải danh sách timesheet.';
        this.isError = true;
      }
    });
  }

  createNewTimesheet() {
    const today = new Date();
    // Về thứ 2 của tuần hiện tại
    const day = today.getDay() || 7;
    today.setDate(today.getDate() - day + 1);
    this.currentWeekStart = new Date(today.getFullYear(), today.getMonth(), today.getDate());

    this.viewMode = 'detail';
    this.loadWeekData();
  }

  viewTimesheet(ts: MyTimesheet) {
    this.currentWeekStart = new Date(ts.weekStartDate);
    this.viewMode = 'detail';
    this.loadWeekData();
  }

  deleteTimesheet(ts: MyTimesheet) {
    if (ts.status === 'Approved') {
      alert('Không thể xoá timesheet đã được Approve.');
      return;
    }

    if (confirm('Bạn có chắc chắn muốn xoá Timesheet tuần ' + ts.weekStartDate.split('T')[0] + ' không?')) {
      this.timesheetService.deleteMyTimesheet(ts.timesheetId).subscribe({
        next: (res) => {
          this.statusMessage = 'Đã xoá Timesheet thành công.';
          this.isError = false;
          setTimeout(() => this.statusMessage = '', 3000);
          this.loadTimesheets();
        },
        error: (err) => {
          this.statusMessage = err.error?.message || 'Lỗi khi xoá';
          this.isError = true;
          setTimeout(() => this.statusMessage = '', 5000);
        }
      });
    }
  }

  backToList() {
    this.viewMode = 'list';
    this.timesheet = null;
    this.loadTimesheets();
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

    // 1. Get tasks (Lấy các task do manager assign trực tiếp cho employee này)
    this.timesheetService.getMyTasks().subscribe({
      next: (tasks) => {
        this.tasks = tasks;
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
        const tInfo = this.tasks.find(t => t.taskItemId === entry.taskItemId);
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
