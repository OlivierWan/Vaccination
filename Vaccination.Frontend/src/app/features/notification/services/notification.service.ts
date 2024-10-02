import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map, Observable, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ApiResponse, INotification } from '../models/notification';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = environment.apiUrl;
  private http = inject(HttpClient);

  countNotifications = signal<number>(0);
  notifications = signal<INotification[]>([]);

  getNotifications(): Observable<INotification[]> {
    const apiUrl = `${this.apiUrl}/Reminder`;
    return this.http.get<ApiResponse>(apiUrl).pipe(
      tap((response) => {
        this.countNotifications.set(response.data.length);
        this.notifications.set(response.data);
      }),
      map((response) => response.data as INotification[]),
    );
  }

  refreshNotifications(): void {
    this.getNotifications().subscribe();
  }
}
