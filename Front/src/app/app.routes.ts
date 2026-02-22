import { Routes } from '@angular/router';
import { LoginComponent } from '../pages/Auth/login/login';
import { RegisterUserComponent } from '../pages/Auth/register-user/register-user';
import { RegisterComponent } from '../pages/Auth/register/register';
import { DashboardComponent } from '../pages/dashboard/dashboard';
import { authGuard } from '../shared/guards/auth.guard';
export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'register-user', component: RegisterUserComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: '', redirectTo: 'register', pathMatch: 'full' },
];
