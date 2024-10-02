import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function passwordMatchValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (
      !password ||
      !confirmPassword ||
      !password.value ||
      !confirmPassword.value
    ) {
      return null;
    }

    // Au lieu de retourner une erreur au niveau du formGroup,
    // on définit l'erreur directement sur le contrôle confirmPassword
    if (password.value !== confirmPassword.value) {
      confirmPassword.setErrors({
        ...confirmPassword.errors,
        passwordMismatch: true,
      });
    } else {
      // Si les mots de passe correspondent, on retire l'erreur passwordMismatch
      const errors = { ...confirmPassword.errors };
      if (errors) {
        delete errors['passwordMismatch'];
        confirmPassword.setErrors(Object.keys(errors).length ? errors : null);
      }
    }

    // On ne retourne plus d'erreur au niveau du formGroup
    return null;
  };
}
