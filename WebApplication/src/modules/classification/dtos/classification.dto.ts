import { ClassificationPublicationDto } from './classification-publication.dto';

export class ClassificationDto {
  id: number;
  name: string;

  organizationId: number | null;
  organizationName: string;

  isExpanded: boolean;
  isLoading: boolean;
  isSelected: boolean;

  children: ClassificationDto[];

  publications: ClassificationPublicationDto[];
}
