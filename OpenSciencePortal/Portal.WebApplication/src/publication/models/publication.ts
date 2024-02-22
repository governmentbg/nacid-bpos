import { SafeUrl } from '@angular/platform-browser';

export class Publication {
  id: number;
  titles: string[];
  creators: string[];
  creatorIdentifiers: string[];
  publishers: string[];
  contributors: string[];
  descriptions: string[];
  subjects: string[];
  contents: string[];
  publicationDate: string;
  publicationYear: number;
  resourceType: string;
  institution: string;
  accessRight: any;
  fundernames: string[];
  awardTitles: string[];
  awardNumbers: string[];
  fundingStreams: string[];
  languages: any[];
  classifications: any[];
  files: { dbId: number, key: string, name: string, url: SafeUrl | string }[];
  fileLocations: any[];
}