import { inject, Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject } from 'rxjs';
import { LocalStorageService } from '../../../core/services/local-storage.service';
import { JwtPayload } from '../models/jwt-payload';

@Injectable({
  providedIn: 'root',
})
export class UserInfoService {
  private localStorageService = inject(LocalStorageService);
  private firstNameSubject = new BehaviorSubject<string>(
    this.getUserFirstName(),
  );

  constructor() {
    this.initializeFromToken();
  }

  private initializeFromToken() {
    const tokenFirstName = this.getUserFirstName();
    this.firstNameSubject.next(tokenFirstName);
  }

  getFirstNameObservable() {
    return this.firstNameSubject.asObservable();
  }

  updateFirstName(newFirstName: string) {
    this.firstNameSubject.next(newFirstName);
  }

  getUserFirstName(): string {
    const decodedToken = this.getDecodedToken();
    return decodedToken ? decodedToken.firstname : '';
  }

  getUserId(): string | null {
    const decodedToken = this.getDecodedToken();
    return decodedToken ? decodedToken.userid : null;
  }

  getUserEmail(): string {
    const decodedToken = this.getDecodedToken();
    return decodedToken ? decodedToken.email : '';
  }

  getToken() {
    return localStorage.getItem('access_token');
  }

  private getDecodedToken(): JwtPayload | null {
    const token = this.getToken();
    if (token) {
      return jwtDecode<JwtPayload>(token);
    }
    return null;
  }
}
