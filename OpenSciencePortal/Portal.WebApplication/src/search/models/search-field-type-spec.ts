import { SearchFieldMode } from './search-field-mode.enum';
import { SearchFieldType } from './search-field-type.enum';

export enum SearchFieldControlType {
  text = 1,
  nomenclature = 2,
  numeric = 3,
  dateRange = 4
}

export class SearchFieldTypeSpec {
  name: string;
  defaultMode: SearchFieldMode;
  defaultValue: any;
  controlType: SearchFieldControlType;
  controlMetadata: object;
}

export const SEARCH_FIELD_SPEC = {
  [SearchFieldType.all]: <SearchFieldTypeSpec>{
    name: 'all',
    defaultMode: SearchFieldMode.allFts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.title]: <SearchFieldTypeSpec>{
    name: 'titles',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.creator]: <SearchFieldTypeSpec>{
    name: 'creators',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.creatorIdentifier]: <SearchFieldTypeSpec>{
    name: 'creatorIdentifiers',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.description]: <SearchFieldTypeSpec>{
    name: 'descriptions',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.subject]: <SearchFieldTypeSpec>{
    name: 'subjects',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.publisher]: <SearchFieldTypeSpec>{
    name: 'publishers',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.awardTitle]: <SearchFieldTypeSpec>{
    name: 'awardTitles',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.awardNumber]: <SearchFieldTypeSpec>{
    name: 'awardNumbers',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.resourceIdentifier]: <SearchFieldTypeSpec>{
    name: 'resourceIdentifier',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.content]: <SearchFieldTypeSpec>{
    name: 'contents',
    defaultMode: SearchFieldMode.fts,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.publicationYear]: <SearchFieldTypeSpec>{
    name: 'publicationYear',
    defaultMode: SearchFieldMode.exact,
    defaultValue: '',
    controlType: SearchFieldControlType.numeric,
    controlMetadata: {}
  },
  [SearchFieldType.accessRight]: <SearchFieldTypeSpec>{
    name: 'accessRight',
    defaultMode: SearchFieldMode.exact,
    defaultValue: null,
    controlType: SearchFieldControlType.nomenclature,
    controlMetadata: {
      restUrl: "AccessRight",
      model: null
    }
  },
  [SearchFieldType.funderName]: <SearchFieldTypeSpec>{
    name: 'funderNames.keyword',
    defaultMode: SearchFieldMode.exact,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.fundingStream]: <SearchFieldTypeSpec>{
    name: 'fundingStreams',
    defaultMode: SearchFieldMode.exact,
    defaultValue: '',
    controlType: SearchFieldControlType.text,
    controlMetadata: {}
  },
  [SearchFieldType.languages]: <SearchFieldTypeSpec>{
    name: 'languages',
    defaultMode: SearchFieldMode.exact,
    defaultValue: null,
    controlType: SearchFieldControlType.nomenclature,
    controlMetadata: {
      restUrl: "Language",
      model: null
    }
  },
  [SearchFieldType.institution]: <SearchFieldTypeSpec>{
    name: 'institution',
    defaultMode: SearchFieldMode.exact,
    defaultValue: null,
    controlType: SearchFieldControlType.nomenclature,
    controlMetadata: {
      restUrl: "Institution",
      model: null
    }
  },
  [SearchFieldType.resourceType]: <SearchFieldTypeSpec>{
    name: 'resourceType',
    defaultMode: SearchFieldMode.exact,
    defaultValue: '',
    controlType: SearchFieldControlType.nomenclature,
    controlMetadata: {
      restUrl: "ResourceType",
      model: null
    }
  },
  [SearchFieldType.classifications]: <SearchFieldTypeSpec>{
    name: 'classifications',
    defaultMode: SearchFieldMode.exact,
    defaultValue: null,
    controlType: SearchFieldControlType.nomenclature,
    controlMetadata: {
      restUrl: "Classification",
      model: null
    }
  }
};
