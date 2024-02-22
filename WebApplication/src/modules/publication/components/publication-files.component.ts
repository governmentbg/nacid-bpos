import { HttpClient, HttpEvent, HttpEventType, HttpRequest } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { PublicationFile } from '../models/publication-file.model';

@Component({
  selector: 'publication-files',
  templateUrl: 'publication-files.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class PublicationFilesComponent {
  @Input() isExpanded = true;

  @Input() files: PublicationFile[] = [];
  @Output() filesChange: EventEmitter<PublicationFile[]> = new EventEmitter();

  @Input() disabled = true;

  @Input() canDeleteUploaded = false;

  constructor(
    private http: HttpClient,
    private cd: ChangeDetectorRef
  ) { }

  deleteUploadedFile(index: number) {
    this.files.splice(index, 1);
    this.filesChange.emit(this.files);
  }

  uploadFile(file: File) {
    const formData = new FormData();

    formData.append('file', file);

    const req = new HttpRequest('POST', 'api/FilesStorage', formData);

    this.http.request<any>(req)
      .subscribe((event: HttpEvent<any>) => {
        if (event.type === HttpEventType.Response) {
          const body = event.body;
          let publicationFile = new PublicationFile();
          publicationFile.dbId = body.dbId;
          publicationFile.key = body.key;
          publicationFile.hash = body.hash;
          publicationFile.size = body.size;
          publicationFile.mimeType = body.mimeType;
          publicationFile.name = body.name;

          if (!this.files || !this.files.length) {
            this.files = [];
          }

          this.files.push(publicationFile);
          this.filesChange.emit(this.files);

          this.cd.markForCheck();
        }
      });
  }
}
