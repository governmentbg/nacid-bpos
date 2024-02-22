import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'publication-step-actions',
  templateUrl: 'publication-step-actions.component.html',
  styles: [`
  :host {
    display: flex;
    justify-content: space-between;
   }`],
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationStepActionsComponent {
  @Input() showBackBtn = true;
  @Input() backBtnText;

  @Input() nextBtnText;

  @Input() disableNextBtn = true;
}
