import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { authGuard } from './auth/auth.guard';
import { TravelingSalesmanComponent } from './traveling-salesman/traveling-salesman.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: '',
    component: TravelingSalesmanComponent,
    canActivate: [authGuard],
    pathMatch: 'full',
  },
  { path: '**', redirectTo: '' },
];
