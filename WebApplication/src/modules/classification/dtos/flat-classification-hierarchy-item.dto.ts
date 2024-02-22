export class FlatClassificationHierarchyItemDto {
  id: number;
  parentId: number | null;

  name: string;

  level: number;

  viewOrder: number;
  hasChildren: boolean;

  isSelected: boolean;
}
