export enum SearchFieldType {
  all = 1,
  title = 2,
  publicationYear = 19,
  creator = 3,
  creatorIdentifier = 4,
  description = 5,
  subject = 6,
  publisher = 7,
  accessRight = 9,
  //Funding Reference
  funderName = 10,
  awardTitle = 11,
  awardNumber = 12,
  fundingStream = 13,
  //-----------------
  languages = 14,
  institution = 15,
  resourceType = 16,
  resourceIdentifier = 17,
  content = 18,

  //new fields
  classifications = 20
}

export const SearchFieldNumberValues: SearchFieldType[] = Object.keys(SearchFieldType)
  .map(key => parseInt(key, 10))
  .filter(key => key >= 0);

export const SearchFieldTypeValuesArray: string[] = SearchFieldNumberValues
  .map(key => SearchFieldType[key]);

export const SearchFieldCategories: SearchFieldType[] = [
  SearchFieldType.institution,
  SearchFieldType.publicationYear,
  SearchFieldType.resourceType,
  SearchFieldType.classifications,
  // SearchFieldType.funderName,
  SearchFieldType.accessRight,
  SearchFieldType.languages
];
