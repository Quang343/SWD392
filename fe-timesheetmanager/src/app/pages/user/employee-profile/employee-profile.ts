import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { EmployeeService, EmployeeProfile as IEmployeeProfile } from '../../../services/employee.service';

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

  constructor(private employeeService: EmployeeService) { }

  ngOnInit(): void {
    this.loadProfile();
    const savedAvatar = localStorage.getItem('userAvatar');
    if (savedAvatar) {
      this.avatarUrl = savedAvatar;
    }
  }

  loadProfile(): void {
    this.isLoading = true;
    this.employeeService.getProfile().subscribe({
      next: (data) => {
        console.log("Dữ liệu BE trả về:", data);
        this.profile = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error("Lỗi khi fetch profile:", err);
        this.isLoading = false;
        this.isError = true;

        // Log lỗi rõ ràng ra UI
        if (err.status === 401) {
          this.message = 'Hết hạn đăng nhập, vui lòng đăng nhập lại!';
        } else if (err.status === 404) {
          this.message = 'Không tìm thấy API (404). Vui lòng Restart lại BE.';
        } else {
          this.message = 'Lỗi hệ thống: ' + (err.error?.message || err.message);
        }
      }
    });
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    if (!this.isEditing) {
      this.loadProfile(); // Reset nếu huỷ
    }
  }

  saveProfile(): void {
    this.isLoading = true;
    this.employeeService.updateProfile({
      fullName: this.profile.fullName,
      department: this.profile.department,
      position: this.profile.position
    }).subscribe({
      next: (res) => {
        this.message = res.message;
        this.isError = false;
        this.isEditing = false;
        this.isLoading = false;
        setTimeout(() => this.message = '', 3000);
      },
      error: (err) => {
        console.error("Lỗi khi update profile:", err);
        this.message = err.error?.message || 'Đã có lỗi xảy ra.';
        this.isError = true;
        this.isLoading = false;
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.avatarUrl = e.target.result;
        localStorage.setItem('userAvatar', this.avatarUrl!);
      };
      reader.readAsDataURL(file);
    }
  }

  triggerFileInput(): void {
    document.getElementById('avatarInput')?.click();
  }
}
