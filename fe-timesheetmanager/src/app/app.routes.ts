import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Register } from './pages/register/register';
import { Manager } from './pages/manager/manager';
import { EmployeeDashboard } from './pages/user/employee-dashboard/employee-dashboard';
import { EmployeeProfile } from './pages/user/employee-profile/employee-profile';

export const routes: Routes = [
  {
    path: 'login',
    component: Login
  },
  {
    path: 'register',
    component: Register
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'manager',
    component: Manager
  },
  {
    path: 'user',
    component: EmployeeDashboard
  },
  {
    path: 'user/profile',
    component: EmployeeProfile
  },
];
