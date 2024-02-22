import { Component, forwardRef, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import * as _moment from 'moment';
const moment = _moment;

@Component({
  selector: 'date-selector',
  templateUrl: './date-selector.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DateSelectorComponent),
      multi: true
    }
  ]
})
export class DateSelectorComponent implements ControlValueAccessor {
  @Input() _dateValue: string;

  @Input() placeholder: string;

  @Input() disabled = true;

  @Input() required = false;

  get dateValue() {
    return moment(new Date(this._dateValue)).format('YYYY-MM-DD');
  }

  set dateValue(val) {
    if (val) {
      this._dateValue = moment(new Date(val)).format('YYYY-MM-DD');
    }

    this.propagateChange(this._dateValue);
  }

  changeDateValue(event: MatDatepickerInputEvent<Date>) {
    this.dateValue = moment(new Date(event.value)).format('YYYY-MM-DD');
  }

  writeValue(value: any) {
    if (value) {
      this.dateValue = moment(new Date(value)).format('YYYY-MM-DD');
    }
  }
  propagateChange = (_: any) => { };

  registerOnChange(fn) {
    this.propagateChange = fn;
  }

  registerOnTouched() { }
}
