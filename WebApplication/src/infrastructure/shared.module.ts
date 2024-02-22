import { NgModule } from '@angular/core';
import { OrderByPipe } from './pipes/order-by.pipe';
import { DayValidationDirective } from './validators/day-validator.directive';
import { EmailValidationDirective } from './validators/email-validator.directive';
import { MinValidatorDirective } from './validators/min-validator.directive';
import { MonthValidationDirective } from './validators/month-validator.directive';
import { PasswordValidationDirective } from './validators/password-validator.directive';
import { YearValidationDirective } from './validators/year-validator.directive';

@NgModule({
  declarations: [
    OrderByPipe,

    PasswordValidationDirective,
    YearValidationDirective,
    MonthValidationDirective,
    DayValidationDirective,
    EmailValidationDirective,
    MinValidatorDirective
  ],
  exports: [
    OrderByPipe,

    PasswordValidationDirective,
    YearValidationDirective,
    MonthValidationDirective,
    DayValidationDirective,
    EmailValidationDirective,
    MinValidatorDirective
  ]
})
export class SharedModule { }
