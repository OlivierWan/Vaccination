import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { NotificationService } from '../../services/notification.service';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Router } from '@angular/router';
import { SnackBarService } from '../../../../core/services/snackbar.service';
import { ISnackBar } from '../../../../core/models/snackbar';

@Component({
  selector: 'app-notification-list',
  standalone: true,
  imports: [
    CommonModule,
    MatListModule,
    MatIconModule,
    MatDividerModule,
    MatTooltipModule,
  ],
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.scss',
})
export class NotificationListComponent implements OnInit {
  private router = inject(Router);
  
  private notificationService = inject(NotificationService);
  private snackBarService = inject(SnackBarService);

  notifications = this.notificationService.notifications;

  ngOnInit() {
    this.notificationService.refreshNotifications();
  }

  onNotificationClick(vaccineCalendarId: string): void {
    if (vaccineCalendarId) {
      this.router.navigate(['/vaccination-calendar-info', vaccineCalendarId]);
    } else {
      const snackBarMessage: ISnackBar = {
        message: 'No vaccine calendar ID found',
        type: 'error',
      };
      this.snackBarService.openSnackbar(snackBarMessage);
    }
  }
}
