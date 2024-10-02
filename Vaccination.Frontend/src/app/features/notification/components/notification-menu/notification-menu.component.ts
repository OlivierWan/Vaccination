import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatBadgeModule } from '@angular/material/badge';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { NotificationService } from '../../services/notification.service';
import { NotificationListComponent } from '../notification-list/notification-list.component';

@Component({
  selector: 'app-notification-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatMenuModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule,
    NotificationListComponent,
  ],
  templateUrl: './notification-menu.component.html',
  styleUrl: './notification-menu.component.scss',
})
export class NotificationMenuComponent implements OnInit {
  private notificationService = inject(NotificationService);

  notificationCount = this.notificationService.countNotifications;

  ngOnInit() {
    this.notificationService.refreshNotifications();
  }
}
