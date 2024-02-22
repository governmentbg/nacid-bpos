import { Component } from "@angular/core";
import { faBars } from '@fortawesome/free-solid-svg-icons';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'ab-menu',
  templateUrl: 'menu.component.html',
  styleUrls: ['menu.component.scss']
})
export class MenuComponent {
  constructor(
    config: Configuration,
    sanitizer: DomSanitizer
  ) {
    this.loginUrl = sanitizer.bypassSecurityTrustUrl(`${config.internalAppUrl}/login`);
  }

  loginUrl: SafeUrl;
  faBars = faBars;
}