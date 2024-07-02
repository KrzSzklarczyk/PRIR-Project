import { Injectable } from '@angular/core';
import {AbstractControl, FormGroup, ValidationErrors, ValidatorFn} from "@angular/forms";


export interface ObjectKeyModel {
  [key: string]: any;
}

export interface PatchModel {
  path: string;
  value: any;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  passwordMatchValidator(form: FormGroup): ValidatorFn {
    return (control: AbstractControl<string>): ValidationErrors | null => {
      let password = form.get('newPassword')?.value;
      if (!password) {
        password = form.get('password')?.value;
      }
      const confirmedPassword = control.value;
      return password === confirmedPassword ? null : { mismatch: true }
    };
  }

  passwordValidator(): ValidatorFn {
    return (control: AbstractControl<string>): ValidationErrors | null => {
      const password = control.value;

      const passwordRegex = /(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z\d])/;
      return passwordRegex.test(password) ? null : { 'passwordInvalid': true }
    };
  }

  validateHTML(form: FormGroup, field: string, error: string, excludeErrors: string[] = []): boolean {
    const hasError = form.get(field)?.hasError(error);
    const excludedErrors = excludeErrors.some(excludeError => form.get(field)?.hasError(excludeError));

    return !!hasError && !excludedErrors;
  }
}
