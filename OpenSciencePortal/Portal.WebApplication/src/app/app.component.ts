import { Component, OnInit, AfterViewInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { LoadingIndicatorService } from 'src/infrastructure/loading-indicator.service';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styles: []
})
export class AppComponent implements AfterViewInit {
  constructor(
    private translate: TranslateService,
    private loadingIndicator: LoadingIndicatorService
  ) {
    this.translate.setDefaultLang('bg');
    this.translate.use('bg');
  }

  isLoading: boolean = false;

  ngAfterViewInit() {
    setTimeout(() => this.loadingIndicator.change$.subscribe((isLoading: boolean) => this.isLoading = isLoading), 100);
  }
}
