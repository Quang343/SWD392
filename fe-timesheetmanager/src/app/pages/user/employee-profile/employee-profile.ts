import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EmployeeService, EmployeeProfile as IEmployeeProfile } from '../../../services/employee.service';
import { timeout, catchError } from 'rxjs/operators';
import { of, throwError } from 'rxjs';

@Component({
  selector: 'app-employee-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './employee-profile.html',
  styleUrl: './employee-profile.css'
})
export class EmployeeProfile implements OnInit {
  profile: IEmployeeProfile = {
    employeeId: 0,
    fullName: '',
    email: '',
    department: '',
    position: '',
    status: '',
    username: ''
  };

  isEditing: boolean = false;
  isLoading: boolean = true;
  avatarUrl: string | null = null;
  message: string = '';
  isError: boolean = false;

  constructor(
    private employeeService: EmployeeService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadProfile();
    const savedAvatar = localStorage.getItem('userAvatar');
    if (savedAvatar) {
      this.avatarUrl = savedAvatar;
    }
  }

  loadProfile(): void {
    this.isLoading = true;
    this.cdr.detectChanges(); // force spinner to show visually

    this.employeeService.getProfile().pipe(
      timeout(10000)
    ).subscribe({
      next: (data) => {
        console.log("Profile Data Successfully Fetched:", data);
        this.profile = data;
        this.isLoading = false;
        this.cdr.detectChanges(); // BẮT BUỘC RENDER LẠI DOM TRÁNH LỖI ZONE.JS
      },
      error: (err) => {
        console.error("HTTP Profile Error:", err);
        this.isLoading = false;
        this.isError = true;

        if (err.status === 401) {
          this.message = 'Hết hạn đăng nhập, vui lòng đăng nhập lại!';
        } else if (err.status === 404) {
          this.message = 'Không tìm thấy API (404). Vui lòng Restart lại BE.';
        } else {
          this.message = 'Lỗi hệ thống: ' + (err.error?.message || err.message);
        }
        this.cdr.detectChanges(); // BẮT BUỘC RENDER LẠI DOM
      }
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.loadProfile(); // Reset nếu huỷ
    }
  }

  isSaving: boolean = false;

  saveProfile(): void {
    this.isSaving = true;
    this.employeeService.updateProfile({
      fullName: this.profile.fullName,
      department: this.profile.department,
      position: this.profile.position
    }).subscribe({
      next: (res) => {
        this.message = res.message || 'Cập nhật thành công!';
        this.isError = false;
        this.isEditing = false;
        this.isSaving = false;
        this.cdr.detectChanges(); // Render mượt mà không bị giật/reload màn hình
        setTimeout(() => this.message = '', 3000);
      },
      error: (err) => {
        this.message = err.error?.message || 'Đã có lỗi xảy ra.';
        this.isError = true;
        this.isSaving = false;
        this.cdr.detectChanges(); 
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.avatarUrl = e.target.result;
        localStorage.setItem('userAvatar', this.avatarUrl || '');
        this.cdr.detectChanges(); // Bắt buộc render DOM ngay lập tức để hiện ảnh mới
      };
      reader.readAsDataURL(file);
    }
  }

  triggerFileInput(): void {
    const input = document.getElementById('avatarInput');
    if (input) {
      input.click();
    }
  }
}
