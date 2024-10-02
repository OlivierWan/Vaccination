import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import { catchError, concatMap, filter, of, Subscription, tap } from 'rxjs';
import { ISnackBar } from '../../../../core/models/snackbar';
import { SnackBarService } from '../../../../core/services/snackbar.service';
import { passwordMatchValidator } from '../../../../core/validators/password-match-validator';
import { LoginRequest } from '../../models/login-request';
import { LoginResponse } from '../../models/login-response';
import { RegisterResponse } from '../../models/register-response';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  private snackBarService = inject(SnackBarService);

  apiErrorMessage: string | null = null;
  registerForm!: FormGroup;

  ngOnInit(): void {
    this.registerForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(10)]],
        confirmPassword: ['', [Validators.required]],
      },
      {
        validators: passwordMatchValidator(),
        updateOn: 'change',
      },
    );
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.subscription.add(
        this.authService
          .register(this.registerForm.value)
          .pipe(
            filter((response: RegisterResponse) => response.success),
            concatMap(() => {
              const loginRequest: LoginRequest = {
                email: this.registerForm.value.email,
                password: this.registerForm.value.password,
              };
              return this.authService.login(loginRequest);
            }),
            tap((loginResponse: LoginResponse) => {
              if (loginResponse.success) {
                const message: ISnackBar = {
                  message: 'Vous êtes connecté',
                  type: 'success',
                };
                this.snackBarService.openSnackbar(message);
                this.router.navigate(['/vaccination']);
              }
            }),
            catchError((error: HttpErrorResponse) => {
              this.apiErrorMessage =
                'Error: ' +
                (error.error?.Message || error.message || 'Unknown error');
              return of(error);
            }),
          )
          .subscribe(),
      );
    } else {
      this.registerForm.markAllAsTouched();
    }
  }

  hasError(controlName: string, errorName: string): boolean {
    return !!this.registerForm.get(controlName)?.errors?.[errorName];
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
