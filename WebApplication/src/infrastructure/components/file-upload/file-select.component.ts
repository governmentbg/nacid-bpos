import { NgTemplateOutlet } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';

@Component({
  selector: 'file-select-button',
  templateUrl: './file-select.component.html'
})
export class FileSelectComponent {

  @ViewChild('inputElem', { static: true }) inputRef: ElementRef<HTMLInputElement>;

  @Input() template: NgTemplateOutlet;

  @Output() fileSelected = new EventEmitter<File>();

  selectFile(ev: Event) {
    ev.stopPropagation();
    if (this.inputRef.nativeElement.files && this.inputRef.nativeElement.files.length) {
      this.fileSelected.emit(this.inputRef.nativeElement.files[0]);
    }
  }
}
