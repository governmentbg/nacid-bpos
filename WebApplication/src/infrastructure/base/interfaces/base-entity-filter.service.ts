export abstract class BaseEntityFilterService {
  limit: number;
  offset: number;

  constructor(defaultLimit: number) {
    this.limit = defaultLimit;
    this.offset = 0;
  }

  abstract clear();
}
