import { Component } from '@angular/core';
import { AuthService, LoginRequest } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  username: string = '';
  password: string = '';
  message: string = '';
  isLoading: boolean = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  // Validate trước khi gửi
  get isFormValid(): boolean {
    return this.username.trim().length >= 3 && this.password.trim().length >= 3;
  }

  onLogin() {
    // Client-side validation
    if (!this.username.trim()) {
      this.message = 'Vui lòng nhập tên tài khoản';
      return;
    }
    if (this.username.trim().length < 3) {
      this.message = 'Tên tài khoản phải có ít nhất 3 ký tự';
      return;
    }
    if (!this.password.trim()) {
      this.message = 'Vui lòng nhập mật khẩu';
      return;
    }
    if (this.password.trim().length < 3) {
      this.message = 'Mật khẩu phải có ít nhất 3 ký tự';
      return;
    }

    this.message = '';
    this.isLoading = true;

    const data: LoginRequest = {
      username: this.username.trim(),
      password: this.password
    };

    this.authService.login(data).subscribe({
      next: (res) => {
        this.isLoading = false;
        // Lưu session
        this.authService.saveSession(res);

        // Redirect theo role
        if (res.role === '2') {
          this.router.navigate(['/manager']);
        } else if (res.role === '4') {
          this.router.navigate(['/user']);
        } else {
          this.router.navigate(['/']);
        }
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401) {
          this.message = 'Sai tài khoản hoặc mật khẩu';
        } else if (err.status === 0) {
          this.message = 'Không thể kết nối đến server. Vui lòng kiểm tra Backend đang chạy.';
        } else {
          this.message = err.error?.message || 'Có lỗi xảy ra, vui lòng thử lại';
        }
      }
    });
  }
}
