import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { LoginRequest } from '../../models/login-request';
import { LoginResponse } from '../../models/login-response';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  apiErrorMessage: string | null = null;

  loginForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(10),
    ]),
  });

  onSubmit(): void {
    if (this.loginForm.valid) {
      const credentials: LoginRequest = {
        email: this.loginForm.value.email || '',
        password: this.loginForm.value.password || '',
      };

      const loginObserver = {
        next: (response: LoginResponse) => {
          if (response.success) {
            this.router.navigate(['/vaccination']);
          } else {
            this.apiErrorMessage = response.message;
          }
        },
        error: (error: HttpErrorResponse) => {
          this.apiErrorMessage = error.error?.Message || 'Unknown error';
        },
      };

      this.authService.login(credentials).subscribe(loginObserver);
    }
  }
}
