import { Routes } from '@angular/router';
import { AdminPanel } from '../pages/admin-panel/admin-panel';
import { LoginComponent } from '../pages/Auth/login/login';
import { RegisterUserComponent } from '../pages/Auth/register-user/register-user';
import { RegisterComponent } from '../pages/Auth/register/register';
import { SubscriptionDashboardComponent } from '../pages/dashboard/dashboard';
import { authGuard } from '../shared/guards/auth.guard';
import { MainLayoutComponent } from '../shared/layout/MainLayout';
export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'register-user', component: RegisterUserComponent },

  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'dashboard', component: SubscriptionDashboardComponent },
      { path: 'AdminPanel', component: AdminPanel },
    ],
  },

  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
