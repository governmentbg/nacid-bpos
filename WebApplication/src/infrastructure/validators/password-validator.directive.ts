import { Directive, forwardRef, Input, OnInit } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[passwordValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => PasswordValidationDirective),
      multi: true
    }
  ]
})
export class PasswordValidationDirective implements Validator, OnInit {
  @Input() passwordValidation: RegExp;

  private valFn: ValidatorFn;

  ngOnInit() {
    this.valFn = PasswordValidator(this.passwordValidation);
  }

  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}

export function PasswordValidator(regex: RegExp): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const invalidResult = { 'invalidPasswordFormat': true };

    const isValidRegex = regex.test(control.value);
    if (control.value && !isValidRegex) {
      return invalidResult;
    }

    return null;
  };
}
