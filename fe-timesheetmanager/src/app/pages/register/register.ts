import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService, RegisterRequest } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  model = {
    username: '',
    fullName: '',
    email: '',
    department: '',
    position: '',
    password: ''
  };
  confirmPassword = '';
  agreedTerms = false;
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  // Email validation regex
  get isEmailValid(): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(this.model.email);
  }

  // Check all fields are valid
  get isFormComplete(): boolean {
    return (
      this.model.username.trim().length >= 3 &&
      this.model.fullName.trim().length >= 2 &&
      this.isEmailValid &&
      this.model.department !== '' &&
      this.model.position !== '' &&
      this.model.password.length >= 6 &&
      this.confirmPassword === this.model.password &&
      this.agreedTerms
    );
  }

  onRegister() {
    this.errorMessage = '';
    this.successMessage = '';

    // Validate từng trường
    if (!this.model.username.trim()) {
      this.errorMessage = 'Vui lòng nhập tên tài khoản';
      return;
    }
    if (this.model.username.trim().length < 3) {
      this.errorMessage = 'Tên tài khoản phải có ít nhất 3 ký tự';
      return;
    }
    if (!this.model.fullName.trim()) {
      this.errorMessage = 'Vui lòng nhập họ và tên';
      return;
    }
    if (!this.model.email.trim()) {
      this.errorMessage = 'Vui lòng nhập email';
      return;
    }
    if (!this.isEmailValid) {
      this.errorMessage = 'Email không hợp lệ';
      return;
    }
    if (!this.model.department) {
      this.errorMessage = 'Vui lòng chọn phòng ban';
      return;
    }
    if (!this.model.position) {
      this.errorMessage = 'Vui lòng chọn vị trí';
      return;
    }
    if (this.model.password.length < 6) {
      this.errorMessage = 'Mật khẩu phải có ít nhất 6 ký tự';
      return;
    }
    if (this.model.password !== this.confirmPassword) {
      this.errorMessage = 'Mật khẩu nhập lại không khớp';
      return;
    }
    if (!this.agreedTerms) {
      this.errorMessage = 'Vui lòng đồng ý với Điều khoản dịch vụ';
      return;
    }

    this.isLoading = true;

    const data: RegisterRequest = {
      username: this.model.username.trim(),
      password: this.model.password,
      fullName: this.model.fullName.trim(),
      email: this.model.email.trim(),
      department: this.model.department,
      position: this.model.position
    };

    this.authService.register(data).subscribe({
      next: () => {
        this.isLoading = false;
        this.successMessage = 'Đăng ký thành công! Đang chuyển đến trang đăng nhập...';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 0) {
          this.errorMessage = 'Không thể kết nối đến server. Vui lòng kiểm tra Backend đang chạy.';
        } else {
          this.errorMessage = err.error?.message || 'Có lỗi xảy ra, vui lòng thử lại';
        }
      }
    });
  }
}
