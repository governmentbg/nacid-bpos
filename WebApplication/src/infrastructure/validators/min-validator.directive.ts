import { Directive, forwardRef, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[min][formControlName],[min][formControl],[min][ngModel]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => MinValidatorDirective),
      multi: true
    }
  ]
})
export class MinValidatorDirective implements OnInit, OnChanges, Validator {
  @Input() min: number;

  private validator: ValidatorFn;
  private onChange: () => void;

  ngOnInit() {
    this.validator = minFunc(this.min);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.min) {
      this.validator = minFunc(changes.min.currentValue);
      if (this.onChange) {
        this.onChange();
      }
    }
  }

  validate(c: AbstractControl): { [key: string]: any } | null {
    return this.validator(c);
  }

  registerOnValidatorChange(fn: () => void): void {
    this.onChange = fn;
  }
}

export const minFunc = (min: number): ValidatorFn => {
  return (control: AbstractControl): { [key: string]: any } | null => {
    if (min === undefined || min === null || control.value === null || control.value === undefined) {
      return null;
    }

    return +control.value >= +min ? null : { 'invalidMinValue': true };
  };
};
