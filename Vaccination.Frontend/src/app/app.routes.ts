import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { AuthService } from './features/auth/services/auth.service';
import { VaccinationCalendarInfoComponent } from './features/vaccination/components/vaccination-calendar-info/vaccination-calendar-info.component';

export const sideNavRoutes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'vaccination',
    loadComponent: () =>
      import(
        './features/vaccination/components/vaccination-list/vaccination-list.component'
      ).then((m) => m.VaccinationListComponent),
    canActivate: [() => inject(AuthService).isLoggedIn()],
    title: 'Carte de vaccination',
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./features/profile/components/profile/profile.component').then(
        (m) => m.ProfileComponent,
      ),
    canActivate: [() => inject(AuthService).isLoggedIn()],
    title: 'Mon profil',
  },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];

export const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: 'home',
    loadComponent: () =>
      import('./features/home/components/home.component').then(
        (m) => m.HomeComponent,
      ),
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/components/login/login.component').then(
        (m) => m.LoginComponent,
      ),
    title: 'Se connecter',
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./features/auth/components/register/register.component').then(
        (m) => m.RegisterComponent,
      ),
    title: 'Se déconnecter',
  },
  {
    path: 'vaccination',
    loadComponent: () =>
      import(
        './features/vaccination/components/vaccination-list/vaccination-list.component'
      ).then((m) => m.VaccinationListComponent),
    canActivate: [() => inject(AuthService).isLoggedIn()],
    title: 'Carte de vaccination',
  },
  {
    path: 'profile',
    loadComponent: () =>
      import('./features/profile/components/profile/profile.component').then(
        (m) => m.ProfileComponent,
      ),
    canActivate: [() => inject(AuthService).isLoggedIn()],
    title: 'Mon profil',
  },
  {
    path: 'vaccination-calendar-info/:vaccineCalendarId',
    component: VaccinationCalendarInfoComponent,
  },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
];
