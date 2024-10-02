import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnDestroy, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Subscription } from 'rxjs';
import { DateService } from '../../../../core/services/date.service';
import { NotificationService } from '../../../notification/services/notification.service';
import { ICalendar } from '../../models/calendar';
import { IVaccination } from '../../models/vaccination';
import { VaccinationService } from '../../services/vaccination.service';

@Component({
  selector: 'app-vaccination-details',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
  ],
  templateUrl: './vaccination-details.component.html',
  styleUrl: './vaccination-details.component.scss',
})
export class VaccinationDetailsComponent implements OnInit, OnDestroy {
  constructor(
    private dialogRef: MatDialogRef<VaccinationDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IVaccination,
  ) {}

  private subscription: Subscription = new Subscription();
  private vaccinationService = inject(VaccinationService);
  private dateService = inject(DateService);
  private notificationService = inject(NotificationService);

  vaccinationTypes: ICalendar[] = [];
  selectedVaccineDescription = '';
  title = '';
  buttonSubmitText = '';

  form = new FormGroup({
    vaccinationType: new FormControl('', Validators.required),
    vaccinationDate: new FormControl<Date | null>(null, Validators.required),
    description: new FormControl(''),
  });

  ngOnInit() {
    this.title = this.data ? 'Modifier un vaccin' : 'Ajouter un vaccin';
    this.buttonSubmitText = this.data ? 'Modifier' : 'Ajouter';

    this.subscription.add(
      (this.data
        ? this.vaccinationService.getAllCalendar()
        : this.vaccinationService.getUserCalendar()
      ).subscribe((response) => {
        this.vaccinationTypes = response;

        if (this.data) {
          this.form.patchValue({
            vaccinationType:
              this.vaccinationTypes.find(
                (v) => v.id == this.data.vaccineCalendarId,
              )?.id || null,
            vaccinationDate: new Date(this.data.vaccinationDate),
            description: this.data.vaccineDescription,
          });

          this.form.get('vaccinationType')?.disable();
        }
      }),
    );

    this.form.get('vaccinationType')?.valueChanges.subscribe((value) => {
      const selectedVaccine = this.vaccinationTypes.find((v) => v.id === value);
      this.selectedVaccineDescription = selectedVaccine
        ? selectedVaccine.description
        : '';
    });
  }
  onSubmit() {
    if (this.form.valid) {
      if (this.data) {
        this.subscription.add(
          this.vaccinationService
            .updateVaccination(this.data.id, {
              vaccinationDate: this.dateService.getDateAsString(
                this.form.value.vaccinationDate,
              ),
              description: this.form.value.description || null,
            })
            .subscribe((response) => {
              this.dialogRef.close(response);
              this.notificationService.refreshNotifications();
            }),
        );
      } else {
        this.subscription.add(
          this.vaccinationService
            .createVaccination({
              vaccinationDate: this.dateService.getDateAsString(
                this.form.value.vaccinationDate,
              ),
              description: this.form.value.description || null,
              vaccineCalendarId: this.form.value.vaccinationType as string,
            })
            .subscribe((response) => {
              this.dialogRef.close(response);
              this.notificationService.refreshNotifications();
            }),
        );
      }
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
