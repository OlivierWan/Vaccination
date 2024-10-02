import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  inject,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import {
  MatTable,
  MatTableDataSource,
  MatTableModule,
} from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { merge, Subscription } from 'rxjs';
import {
  debounceTime,
  distinctUntilChanged,
  map,
  startWith,
  switchMap,
} from 'rxjs/operators';
import { ISnackBar } from '../../../../core/models/snackbar';
import { SnackBarService } from '../../../../core/services/snackbar.service';
import { IVaccination } from '../../models/vaccination';
import { VaccinationService } from '../../services/vaccination.service';
import { VaccinationDetailsComponent } from '../vaccination-details/vaccination-details.component';

@Component({
  selector: 'app-vaccination-list',
  standalone: true,
  imports: [
    CommonModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatButtonModule,
    MatTooltipModule,
    MatToolbarModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatCardModule,
    FormsModule,
  ],
  templateUrl: './vaccination-list.component.html',
  styleUrl: './vaccination-list.component.scss',
})
export class VaccinationListComponent implements AfterViewInit, OnDestroy {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  private subscription: Subscription = new Subscription();

  private cdr = inject(ChangeDetectorRef);
  private dialog = inject(MatDialog);

  private vaccinationService = inject(VaccinationService);
  private snackBarService = inject(SnackBarService);

  displayedColumns: string[] = [
    'vaccinationDate',
    'vaccineName',
    'vaccineDescription',
  ];

  dataSource = new MatTableDataSource<IVaccination>([]);
  searchControl = new FormControl('');

  resultsLength = 0;
  isLoadingResults = true;

  @ViewChild(MatTable) table!: MatTable<IVaccination>;

  ngAfterViewInit() {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    this.subscription.add(
      merge(
        this.sort.sortChange,
        this.paginator.page,
        this.searchControl.valueChanges.pipe(
          debounceTime(300),
          distinctUntilChanged(),
        ),
      )
        .pipe(
          startWith({}),
          switchMap((value) => {
            this.isLoadingResults = true;
            this.cdr.markForCheck();
            let searchTerm = typeof value === 'string' ? value : '';
            if (searchTerm.length < 3) {
              // If searchTerm is less than 3 characters, consider it as an empty string to avoid too many useless results.
              searchTerm = '';
            }
            return this.vaccinationService.getVaccinations(
              this.sort.active,
              this.sort.direction,
              this.paginator.pageIndex,
              this.paginator.pageSize,
              searchTerm,
            );
          }),
          map((data) => {
            this.isLoadingResults = false;
            this.resultsLength = data.totalCount;
            this.cdr.markForCheck();
            return data.vaccinations;
          }),
        )
        .subscribe((data) => {
          this.dataSource.data = data;
        }),
    );
  }

  refreshVaccinations() {
    this.subscription.add(
      this.vaccinationService
        .getVaccinations(
          this.sort.active,
          this.sort.direction,
          this.paginator.pageIndex,
          this.paginator.pageSize,
          this.searchControl.value || '',
        )
        .subscribe((data) => {
          this.dataSource.data = data.vaccinations;
          this.resultsLength = data.totalCount;
          this.cdr.markForCheck();
        }),
    );
  }

  onSelect(vaccination: IVaccination) {
    const dialogRef = this.dialog.open(VaccinationDetailsComponent, {
      data: vaccination,
    });

    dialogRef.afterClosed().subscribe((result) => {
      const notification: ISnackBar = {
        message: result,
        type: 'success',
      };
      if (result) {
        this.snackBarService.openSnackbar(notification);
        this.refreshVaccinations();
      }
    });
  }

  add() {
    const dialogRef = this.dialog.open(VaccinationDetailsComponent, {});

    dialogRef.afterClosed().subscribe((result) => {
      const notification: ISnackBar = {
        message: result,
        type: 'success',
      };

      if (result) {
        this.snackBarService.openSnackbar(notification);
        this.refreshVaccinations();
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
