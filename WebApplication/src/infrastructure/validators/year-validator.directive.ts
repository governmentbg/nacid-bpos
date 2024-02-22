import { Directive, forwardRef, Input, OnInit } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[yearValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => YearValidationDirective),
      multi: true
    }
  ]
})
export class YearValidationDirective implements Validator, OnInit {
  @Input() maxYear: number | null;
  @Input() minYear: number | null;

  private valFn: ValidatorFn;

  ngOnInit() {
    this.valFn = YearValidator(this.minYear, this.maxYear);
  }

  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}

export function YearValidator(minYear: number | null, maxYear: number | null): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    // tslint:disable-next-line: max-line-length
    const invalidResult = { 'invalidYear': true };

    if (control.value === null || control.value === undefined) {
      return null;
    }

    const upperBound = maxYear || new Date().getFullYear();
    if (upperBound < control.value
      || (minYear && control.value < minYear)
    ) {
      return invalidResult;
    }

    return null;
  };
}
