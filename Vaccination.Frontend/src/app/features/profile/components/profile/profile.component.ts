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
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router } from '@angular/router';
import {
  NgxMaterialIntlTelInputComponent,
  TextLabels,
} from 'ngx-material-intl-tel-input';
import { catchError, of, Subscription } from 'rxjs';
import { ISnackBar } from '../../../../core/models/snackbar';
import { DateService } from '../../../../core/services/date.service';
import { SnackBarService } from '../../../../core/services/snackbar.service';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { AuthService } from '../../../auth/services/auth.service';
import { UserInfoService } from '../../../auth/services/user-info.service';
import { NotificationService } from '../../../notification/services/notification.service';
import { ProfileUserResponse } from '../../models/profile-user-response';
import { ProfileUserUpdateRequest } from '../../models/profile-user-update';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatDialogModule,
    NgxMaterialIntlTelInputComponent,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit, OnDestroy {
  private subscription: Subscription = new Subscription();

  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  private snackBarService = inject(SnackBarService);
  private userInfoService = inject(UserInfoService);
  private profileService = inject(ProfileService);
  private authService = inject(AuthService);
  private dateService = inject(DateService);
  private notificationService = inject(NotificationService);

  apiErrorMessage: string | null = null;
  profileForm!: FormGroup;

  // translate labels for ngx-material-intl-tel-input
  textPhone: TextLabels = {
    codePlaceholder: 'Code',
    hintLabel: '',
    invalidNumberError: 'Numéro de téléphone invalide',
    mainLabel: '',
    nationalNumberLabel: 'Numéro de téléphone',
    noEntriesFoundLabel: 'Aucune entrée trouvée',
    requiredError: 'Numéro de téléphone requis',
    searchPlaceholderLabel: 'Rechercher',
  };

  ngOnInit() {
    this.profileForm = this.fb.group({
      email: [{ value: '', disabled: true }],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      socialSecurityNumber: [
        '',
        [Validators.minLength(13), Validators.maxLength(13)],
      ],
      dateOfBirth: [''],
      city: [''],
      nationality: [''],
      address: [''],
      postalCode: [''],
      phoneNumber: [''],
    });

    this.loadUserProfile();
  }

  loadUserProfile(): void {
    this.subscription.add(
      this.profileService
        .getUserProfile()
        .pipe(
          catchError((error: HttpErrorResponse) => {
            this.apiErrorMessage = error.error?.Message || 'Unknown error';
            return of(null);
          }),
        )
        .subscribe({
          next: (profile: ProfileUserResponse | null) => {
            if (profile) {
              this.profileForm.patchValue(profile);
            }
          },
          error: (error: HttpErrorResponse) => {
            this.apiErrorMessage = error.error?.Message || 'Unknown error';
          },
        }),
    );
  }

  saveProfile(): void {
    if (this.profileForm.valid) {
      // Enable field temporarily to update the profile and disable it again
      this.profileForm.get('email')?.enable();
      const profile: ProfileUserUpdateRequest = {
        email: this.profileForm.value.email,
        firstName: this.profileForm.value.firstName,
        lastName: this.profileForm.value.lastName,
        socialSecurityNumber: this.profileForm.value.socialSecurityNumber,
        dateOfBirth: this.dateService.getDateAsString(
          this.profileForm.value.dateOfBirth,
        ),
        city: this.profileForm.value.city,
        address: this.profileForm.value.address,
        postalCode: this.profileForm.value.postalCode,
        phoneNumber: this.profileForm.value.phoneNumber,
        nationality: this.profileForm.value.nationality,
      };
      this.profileForm.get('email')?.disable();
      this.subscription.add(
        this.profileService.updateUserProfile(profile).subscribe({
          next: () => {
            this.apiErrorMessage = null;
            this.userInfoService.updateFirstName(profile.firstName);
            this.notificationService.refreshNotifications();
            const message: ISnackBar = {
              message: 'Profil mis à jour avec succès.',
              type: 'success',
            };
            this.snackBarService.openSnackbar(message);
          },
          error: (error: HttpErrorResponse) => {
            this.apiErrorMessage = error.error?.Message || 'Unknown error';
          },
        }),
      );
    }
  }

  confirmDeleteAccount(): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Supprimer le compte',
        message:
          'Êtes-vous sûr de vouloir supprimer votre compte ? Cette action est irréversible.',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.deleteAccount();
        const message: ISnackBar = {
          message: 'Votre compte a été supprimé avec succès.',
          type: 'success',
        };
        this.snackBarService.openSnackbar(message);
      }
    });
  }

  deleteAccount(): void {
    this.subscription.add(
      this.profileService.deleteUserProfile().subscribe({
        next: () => {
          this.apiErrorMessage = null;
          this.authService.logout();
          this.router.navigate(['/login']);
        },
        error: (error: HttpErrorResponse) => {
          this.apiErrorMessage = error.error?.Message || 'Unknown error';
        },
      }),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
