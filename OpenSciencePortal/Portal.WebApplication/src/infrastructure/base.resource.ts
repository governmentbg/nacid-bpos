import { HttpClient } from '@angular/common/http';
import { Configuration } from './configuration/configuration';
import { Optional } from '@angular/core';

export class BaseResource {
  protected baseUrl: string;

  constructor(
    protected http: HttpClient,
    protected configuration: Configuration,
    protected suffix?: string
  ) {
    this.baseUrl = `${this.configuration.restUrl}`;
    this.setSuffix(suffix);
  }

  public getBaseUrl(): string {
    return this.baseUrl;
  }

  public setSuffix(suffix?: string): void {
    if (suffix) {
      this.baseUrl = `${this.configuration.restUrl}/${suffix}`;
    }
  }

  public composeQueryString(object: any): string {
    let result = '';
    let isFirst = true;

    if (object) {
      Object.keys(object).forEach(key => {
        if (isFirst) {
          result = '?' + key + '=' + object[key];
          isFirst = false;
        } else {
          result += '&' + key + '=' + object[key];
        }
      });
    }

    return result;
  }
}
