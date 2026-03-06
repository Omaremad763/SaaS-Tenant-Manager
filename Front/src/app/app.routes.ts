import { Routes } from '@angular/router';
import { AdminPanel } from '../pages/admin-panel/admin-panel';
import { LoginComponent } from '../pages/Auth/login/login';
import { RegisterUserComponent } from '../pages/Auth/register-user/register-user';
import { RegisterComponent } from '../pages/Auth/register/register';
import { ClientManagementComponent } from '../pages/client/client';
import { CreateShipmentWizardComponent } from '../pages/create-shipment-wizard/create-shipment-wizard';
import { SubscriptionDashboardComponent } from '../pages/dashboard/dashboard';
import { ShipmentManagementComponent } from '../pages/shipment/shipment';
import { authGuard } from '../shared/guards/auth.guard';
import { MainLayoutComponent } from '../shared/layout/MainLayout';
export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
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
      { path: 'Shipments', component: ShipmentManagementComponent },
      { path: 'Clients', component: ClientManagementComponent },
      { path: 'shipmentwizard', component: CreateShipmentWizardComponent },
    ],
  },

  { path: '**', redirectTo: 'login' },
];
