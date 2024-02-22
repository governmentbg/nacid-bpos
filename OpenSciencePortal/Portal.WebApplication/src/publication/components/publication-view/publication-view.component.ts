import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Configuration } from 'src/infrastructure/configuration/configuration';
import { Publication } from '../../models/publication';
import { faFilePdf } from '@fortawesome/free-solid-svg-icons';


@Component({
  templateUrl: 'publication-view.component.html',
  styleUrls: ['publication-view.component.scss']
})
export class PublicationViewComponent {
  constructor(
    private route: ActivatedRoute
  ) {
    this.route.data.subscribe((data: { publication: Publication }) => this.publication = data.publication);
  }

  publication: Publication;
  faFilePdf = faFilePdf;
}