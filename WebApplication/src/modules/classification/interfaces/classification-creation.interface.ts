import { ClassificationDto } from '../dtos/classification.dto';

export interface IClassificationCreation {
  id: number | undefined;
  parentClassification: ClassificationDto;
}
