import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { LanguageDto } from '../language-select/dtos/language.dto';
import { IMenuItem } from './interfaces/menu-item.interface';

@Component({
  selector: 'app-menu',
  templateUrl: 'app-menu.component.html',
  styleUrls: ['./app-menu.styles.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class AppMenuComponent {
  @Input() items: IMenuItem[];

  @Input() languages: LanguageDto[];

  isCollapsed = false;

  constructor(private router: Router) { }

  navigateTo(item: IMenuItem) {
    this.items.forEach(item => item.isSelected = false);
    item.isSelected = true;
    this.isCollapsed = false;
    this.router.navigate([item.route]);
  }
}
