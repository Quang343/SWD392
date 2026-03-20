import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Manager } from './pages/manager/manager';
import { User } from './pages/user/user';

export const routes: Routes = [
    {
    path: 'login',
    component: Login
    },
    {
    path: '',
    component: Login
    },
    { path: 'login', 
    component: Login 
    },
    { path: 'manager',    
    component: Manager 
    },
    { path: 'user', 
    component: User 
    },
];
