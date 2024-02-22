import { Directive, forwardRef, Input, OnInit } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[dayValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => DayValidationDirective),
      multi: true
    }
  ]
})
export class DayValidationDirective implements Validator, OnInit {
  year: number | null;
  @Input('year') set yearSetter(value: number | null) {
    this.year = value;
    this.valFn = DayValidator(value, this.month);
  }
  month: number | null;
  @Input('month') set monthSetter(value: number | null) {
    this.month = value;
    this.valFn = DayValidator(this.year, value);
  }

  private valFn: ValidatorFn;

  ngOnInit() {
    this.valFn = DayValidator(this.year, this.month);
  }

  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}

export function DayValidator(year: number | null, month: number | null): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    // tslint:disable-next-line: max-line-length
    const invalidResult = { 'invalidDay': true };

    if (control.value === null || control.value === undefined) {
      return null;
    }

    let upperBound = 31;
    switch (month) {
      case 2:
        if ((year % 4 == 0 && year % 100) || year % 400 == 0) {
          upperBound = 29;
        } else {
          upperBound = 28;
        }
        break;
      case 4:
      case 6:
      case 9:
      case 11:
        upperBound = 30;
        break;
    }
    if (control.value < 1 || control.value > upperBound) {
      return invalidResult;
    }

    return null;
  };
}
