import { Directive, forwardRef, OnInit } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[monthValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => MonthValidationDirective),
      multi: true
    }
  ]
})
export class MonthValidationDirective implements Validator, OnInit {
  private valFn: ValidatorFn;

  ngOnInit() {
    this.valFn = MonthValidator();
  }

  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}

export function MonthValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    // tslint:disable-next-line: max-line-length
    const invalidResult = { 'invalidMonth': true };

    if (control.value === null || control.value === undefined) {
      return null;
    }

    if (control.value < 1 || control.value > 12) {
      return invalidResult;
    }

    return null;
  };
}
