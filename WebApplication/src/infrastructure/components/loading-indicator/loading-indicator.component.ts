import { Component } from '@angular/core';
import { Configuration } from 'src/infrastructure/base/configuration/configuration';
import { LoadingIndicatorService } from './loading-indicator.service';

@Component({
  selector: 'loading-indicator',
  templateUrl: 'loading-indicator.component.html',
  styleUrls: ['loading-indicator.style.css']
})

export class LoadingIndicatorComponent {
  isLoading: boolean;
  private loadingCount = 0;

  constructor(
    private loadingService: LoadingIndicatorService,
    private configuration: Configuration
  ) {
    this.isLoading = false;
    this.loadingCount = 0;

    this.loadingService.subscribe((load: boolean) => {
      if (load) {
        this.loadingCount++;
      } else {
        this.loadingCount = this.loadingCount - 1 >= 0 ? this.loadingCount - 1 : 0;
      }

      setTimeout(() => {
        this.isLoading = this.loadingCount !== 0;
      }, this.configuration.loadingTimeoutInSeconds * 1000);
    });
  }
}
