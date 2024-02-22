import { Directive, forwardRef, Input, OnInit } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[emailValidation]',
  providers: [
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => EmailValidationDirective),
      multi: true
    }
  ]
})
export class EmailValidationDirective implements Validator, OnInit {
  @Input() acceptNull = false;

  private valFn: ValidatorFn;

  ngOnInit() {
    this.valFn = EmailValidator(this.acceptNull);
  }

  validate(control: AbstractControl): { [key: string]: any } | null {
    return this.valFn(control);
  }
}

export function EmailValidator(acceptNull: boolean): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    // tslint:disable-next-line: max-line-length
    const EMAIL_REGEXP = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    const invalidResult = { 'invalidEmail': true };

    const isValidRegex = EMAIL_REGEXP.test(control.value);
    if (!acceptNull) {
      if (!isValidRegex) {
        return invalidResult;
      }

      return null;
    } else {
      if (control.value && !isValidRegex) {
        return invalidResult;
      }

      return null;
    }
  };
}
