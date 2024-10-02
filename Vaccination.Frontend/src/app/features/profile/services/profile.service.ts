import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  ApiResponse,
  ProfileUserResponse,
} from '../models/profile-user-response';
import { ProfileUserUpdateRequest } from '../models/profile-user-update';

@Injectable({
  providedIn: 'root',
})
export class ProfileService {
  private apiUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getUserProfile(): Observable<ProfileUserResponse> {
    const url = `${this.apiUrl}/user`;
    return this.http.get<ApiResponse>(url).pipe(
      map((response: ApiResponse) => {
        const data = response.data;
        const profile: ProfileUserResponse = {
          id: data.id || '',
          email: data.email || '',
          firstName: data.firstName || '',
          lastName: data.lastName || '',
          socialSecurityNumber: data.socialSecurityNumber || '',
          dateOfBirth: data.dateOfBirth || null,
          city: data.city || '',
          address: data.address || '',
          postalCode: data.postalCode || '',
          phoneNumber: data.phoneNumber || '',
          nationality: data.nationality || '',
        };
        return profile;
      }),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => new Error(error.message));
      }),
    );
  }

  updateUserProfile(profile: ProfileUserUpdateRequest): Observable<void> {
    const url = `${this.apiUrl}/user`;
    return this.http.put<ApiResponse>(url, profile).pipe(
      map(() => void 0),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => new Error(error.message));
      }),
    );
  }

  deleteUserProfile(): Observable<void> {
    const url = `${this.apiUrl}/user`;
    return this.http.delete(url).pipe(
      map(() => void 0),
      catchError((error: HttpErrorResponse) => {
        return throwError(() => new Error(error.message));
      }),
    );
  }
}
