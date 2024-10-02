import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { ICalendar } from '../../models/calendar';
import { VaccinationService } from '../../services/vaccination.service';

@Component({
  selector: 'app-vaccination-calendar-info',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './vaccination-calendar-info.component.html',
  styleUrl: './vaccination-calendar-info.component.scss',
})
export class VaccinationCalendarInfoComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private vaccinationService = inject(VaccinationService);

  calendarId: string | null = null;
  calendarData: ICalendar | null = null;
  errorMessage: string | null = null;

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.calendarId = params.get('vaccineCalendarId');
      if (this.calendarId) {
        this.loadCalendarData(this.calendarId);
      }
    });
  }

  loadCalendarData(id: string): void {
    this.vaccinationService
      .getAllCalendar()
      .pipe(
        map((data) => data.find((calendar) => calendar.id === id) || null),
        catchError((error) => {
          this.errorMessage = error.message;
          return of(null);
        }),
      )
      .subscribe((calendar) => {
        this.calendarData = calendar;
      });
  }
}
