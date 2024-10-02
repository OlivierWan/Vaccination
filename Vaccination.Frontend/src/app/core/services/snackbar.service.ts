import { inject, Injectable } from '@angular/core';
import {
  MatSnackBar,
  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition,
} from '@angular/material/snack-bar';
import {
  SnackBarComponent
} from '../../shared/components/snackbar/snackbar.component';
import { ISnackBar } from '../models/snackbar';

@Injectable({
  providedIn: 'root',
})
export class SnackBarService {
  private snackbar = inject(MatSnackBar);

  horizontalPosition: MatSnackBarHorizontalPosition = 'center';
  verticalPosition: MatSnackBarVerticalPosition = 'top';

  public openSnackbar(snackBar: ISnackBar): void {
    this.snackbar.openFromComponent(SnackBarComponent, {
      data: snackBar,
      panelClass: ['snackbar-' + snackBar.type],
      duration: 3000,
      horizontalPosition: this.horizontalPosition,
      verticalPosition: this.verticalPosition,
    });
  }
}
