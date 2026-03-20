import { Component } from '@angular/core';
import { AuthService, LoginRequest } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  username: string = '';
  password: string = '';
  message: string = '';

  constructor(
  private authService: AuthService,
  private router: Router
) {}

  onLogin() {
    const data: LoginRequest = {
      username: this.username,
      password: this.password
    };

    this.authService.login(data).subscribe({
    next: (res) => {
      // lưu token + role
      localStorage.setItem('token', res.token);
      localStorage.setItem('role', res.role);

      // 🔥 redirect theo role
      if (res.role === "2") {
        this.router.navigate(['/manager']);
      } else if (res.role === "4") {
        this.router.navigate(['/user']);
      }
    },
    error: () => {
      this.message = 'Sai tài khoản hoặc mật khẩu';
    }
  });
}}
