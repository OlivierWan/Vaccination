import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule } from '@angular/router';
import { map, Observable, shareReplay } from 'rxjs';
import { sideNavRoutes } from '../../app.routes';
import { AuthService } from '../../features/auth/services/auth.service';
import { UserInfoService } from '../../features/auth/services/user-info.service';
import { NotificationMenuComponent } from '../../features/notification/components/notification-menu/notification-menu.component';
import { SnackBarComponent } from '../../shared/components/snackbar/snackbar.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    NotificationMenuComponent,
    SnackBarComponent,
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
})
export class LayoutComponent implements OnInit {
  private authService = inject(AuthService);
  private userInfoService = inject(UserInfoService);
  private breakpointObserver = inject(BreakpointObserver);
  private router = inject(Router);

  isHandset = signal(false);
  isLoggedIn = this.authService.isLoggedIn;
  userFirstname = '';

  sideNavRoutes = sideNavRoutes.filter((r) => r.path);

  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(
      map((result) => result.matches),
      shareReplay(),
    );

  ngOnInit(): void {
    this.userInfoService.getFirstNameObservable().subscribe((firstName) => {
      this.userFirstname = firstName;
    });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
