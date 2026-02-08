import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { AdminDashboardComponent } from './pages/admin-dashboard/admin-dashboard.component';
import { adminGuard } from './core/guards/admin-guard';
import { ProviderDetailsComponent } from './pages/provider-details/provider-details.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent },
  {
    path: 'admin',
    component: AdminDashboardComponent,
    canActivate: [adminGuard] // Protect this route
  },
  { path: 'view/:id', component: ProviderDetailsComponent }, // Public Route
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
