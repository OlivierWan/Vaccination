import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { ICalendar } from '../models/calendar';
import { ICreateVaccination } from '../models/create-vaccination';
import { IUpdateVaccination } from '../models/update-vaccination';
import { IVaccination, IVaccinationList } from '../models/vaccination';

@Injectable({
  providedIn: 'root',
})
export class VaccinationService {
  private apiUrl = environment.apiUrl;

  private http = inject(HttpClient);

  getVaccinations(
    orderBy: string,
    orderDirection: string,
    pageNumber: number,
    pageSize: number,
    criteria: string,
  ): Observable<IVaccinationList> {
    const apiUrl = `${this.apiUrl}/vaccination`;
    return this.http
      .get<{ success: boolean; data: IVaccination[]; message: string }>(
        apiUrl,
        {
          params: {
            orderBy: orderBy,
            orderDirection: orderDirection,
            pageNumber: (pageNumber + 1).toString(),
            pageSize: pageSize.toString(),
            criteriaSearch: criteria,
          },
          observe: 'response',
        },
      )
      .pipe(
        map((response) => {
          const pagination = JSON.parse(
            response.headers.get('X-Pagination') || '{}',
          );

          return {
            vaccinations:
              response.body?.data.map((item) => ({
                id: item.id,
                vaccineName: item.vaccineName,
                vaccinationDate: item.vaccinationDate,
                vaccineDescription: item.vaccineDescription,
                vaccineCalendarId: item.vaccineCalendarId,
              })) || [],
            totalCount: pagination.TotalCount || 0,
          } as IVaccinationList;
        }),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => new Error(error.message));
        }),
      );
  }

  getUserCalendar(): Observable<ICalendar[]> {
    const apiUrl = `${this.apiUrl}/calendar?next=true`;
    return this.http
      .get<{
        success: boolean;
        data: ICalendar[];
      }>(apiUrl)
      .pipe(
        map((response) => response.data),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => new Error(error.message));
        }),
      );
  }

  getAllCalendar(): Observable<ICalendar[]> {
    const apiUrl = `${this.apiUrl}/calendar?next=false`;
    return this.http
      .get<{
        success: boolean;
        data: ICalendar[];
      }>(apiUrl)
      .pipe(
        map((response) => response.data),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => new Error(error.message));
        }),
      );
  }

  createVaccination(vaccination: ICreateVaccination): Observable<string> {
    const apiUrl = `${this.apiUrl}/vaccination`;
    return this.http
      .post<{
        success: boolean;
        data: IVaccination;
        message: string;
      }>(apiUrl, vaccination)
      .pipe(
        map((response) => response.message),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => new Error(error.message));
        }),
      );
  }

  updateVaccination(
    id: string,
    vaccination: IUpdateVaccination,
  ): Observable<string> {
    const apiUrl = `${this.apiUrl}/vaccination/${id}`;
    return this.http
      .put<{
        success: boolean;
        data: IVaccination;
        message: string;
      }>(apiUrl, vaccination)
      .pipe(
        map((response) => response.message),
        catchError((error: HttpErrorResponse) => {
          return throwError(() => new Error(error.message));
        }),
      );
  }
}
