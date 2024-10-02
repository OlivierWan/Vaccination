import { HttpBackend, HttpClient } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { jwtDecode } from 'jwt-decode';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { LocalStorageService } from '../../../core/services/local-storage.service';
import { JwtPayload } from '../models/jwt-payload';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { RefreshTokenResponse } from '../models/refresh-token-response';
import { RegisterRequest } from '../models/register-request';
import { RegisterResponse } from '../models/register-response';
import { UserInfoService } from './user-info.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private localStorageService: LocalStorageService,
    private userInfoService: UserInfoService,
    private handler: HttpBackend,
  ) {
    this.http = new HttpClient(handler);
    this.refreshToken$.subscribe(() => {
      this.refreshToken();
    });
    this.loadUserFromLocalStorage();
  }

  readonly isLoggedIn = computed(() => this.isLoggedInSignal());

  private apiUrl = environment.apiUrl;
  public refreshToken$ = new BehaviorSubject<boolean>(false);

  private isLoggedInSignal = signal<boolean>(false);

  private loadUserFromLocalStorage() {
    const token = this.getToken();
    if (token) {
      const decodedToken = this.getDecodedToken();
      if (decodedToken) {
        this.isLoggedInSignal.set(true);
      }
    }
  }

  getToken() {
    return localStorage.getItem('access_token');
  }

  getRefreshToken() {
    return localStorage.getItem('refresh_token');
  }

  private getDecodedToken(): JwtPayload | null {
    const token = this.getToken();
    if (token) {
      return jwtDecode<JwtPayload>(token);
    }
    return null;
  }

  login(loginRequest: LoginRequest): Observable<LoginResponse> {
    const url = `${this.apiUrl}/auth/login`;
    return this.http.post<LoginResponse>(url, loginRequest).pipe(
      tap((response) => {
        if (response.success && response.data) {
          this.localStorageService.setItem('access_token', response.data.token);
          this.localStorageService.setItem(
            'refresh_token',
            response.data.refreshToken,
          );
          this.isLoggedInSignal.set(true);
          const firstName = this.userInfoService.getUserFirstName();
          this.userInfoService.updateFirstName(firstName);
        }
      }),
    );
  }

  logout(): void {
    this.isLoggedInSignal.set(false);
    this.localStorageService.removeItem('access_token');
    this.localStorageService.removeItem('refresh_token');
  }

  refreshToken() {
    const url = `${this.apiUrl}/auth/refresh-token`;
    this.http
      .post<RefreshTokenResponse>(url, {
        accessToken: this.getToken(),
        refreshToken: this.getRefreshToken(),
      })
      .subscribe((res: RefreshTokenResponse) => {
        if (res.success && res.data) {
          this.localStorageService.setItem('access_token', res.data.token);
          this.localStorageService.setItem(
            'refresh_token',
            res.data.refreshToken,
          );
        }
      });
  }

  register(registerRequest: RegisterRequest): Observable<RegisterResponse> {
    const url = `${this.apiUrl}/auth/register`;
    return this.http.post<RegisterResponse>(url, registerRequest).pipe(
      map((response: RegisterResponse) => {
        return {
          success: response.success,
          message: response.message,
        } as RegisterResponse;
      }),
    );
  }
}
