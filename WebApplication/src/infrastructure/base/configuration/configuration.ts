import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class Configuration {
  readonly tokenProperty = 'access_token';
  readonly userIdProperty = 'user_id';
  readonly userFullnameProperty = 'user_fullname';
  readonly userRolesProperty = 'user_roles';
  readonly userInstitutionsProperty = 'user_institutions';

  clientUrl: string;
  restUrl: string;

  loadingTimeoutInSeconds: number;

  constructor(
    private httpClient: HttpClient
  ) { }

  load(): Promise<{}> {
    return new Promise((resolve) => {
      this.httpClient.get('./../../../configuration.json')
        .subscribe((e) => {
          this.importSettings(e);
          resolve(true);
        });
    });
  }

  private importSettings(config: any) {
    if (config.restUrlFromAppOrigin) {
      config.urls.restUrl = `${window.location.origin}/api`;
      config.urls.clientUrl = `${window.location.origin}`;
    }

    this.restUrl = config.urls.restUrl;
    this.clientUrl = config.urls.clientUrl;
    this.loadingTimeoutInSeconds = config.loadingTimeoutInSeconds;
  }
}

export function configurationFactory(configuration: Configuration): Function {
  return () => configuration.load();
}
