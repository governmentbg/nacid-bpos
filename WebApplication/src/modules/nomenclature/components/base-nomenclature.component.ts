import { MatDialog } from '@angular/material/dialog';
import { Nomenclature } from 'src/infrastructure/base/models/nomenclature.model';
import { ConfirmDialogComponent } from 'src/infrastructure/components/confirm-modal/confirm-modal.component';
import { NomenclatureResource } from '../nomenclature.resource';

export class BaseNomenclatureComponent<T extends Nomenclature> {
  model: T[] = [];

  private type: (new () => T);

  constructor(
    protected resource: NomenclatureResource<T>,
    protected dialog: MatDialog
  ) {
  }

  protected init(type: (new () => T), prefix: string) {
    this.type = type;
    this.resource.setSuffix(prefix);

    this.resource.getFiltered()
      .subscribe((model: T[]) => this.model = model);
  }

  add() {
    if (this.model.filter(e => e.isEditMode).length) {
      return;
    }

    const newEntity = new this.type();
    newEntity.isActive = true;
    newEntity.isEditMode = true;

    this.model.push(newEntity);
  }

  edit(item: T) {
    item.originalObject = Object.assign({}, item);
    item.isEditMode = true;
  }

  cancel(item: T, index: number) {
    if (!item.id) {
      this.model.splice(index, 1);
    } else {
      Object.keys(item).forEach(key => {
        if (key !== 'originalObject') {
          item[key] = item.originalObject[key];
        }
      });

      item.isEditMode = false;
      item.originalObject = null;
    }
  }

  save(item: T, index: number) {
    item.originalObject = null;

    if (!item.id) {
      return this.resource.add(item)
        .subscribe((result: T) => this.model[index] = result);
    } else {
      return this.resource.update(item.id, item)
        .subscribe((result: T) => this.model[index] = result);
    }
  }

  delete(id: number, index: number) {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        header: 'Изтриване на запис',
        body: 'Сигурни ли сте, че искате да изтриете избрания запис?',
        acceptText: 'Изтрий',
        showDecline: true,
        declineText: 'Откажи'
      }
    })
      .afterClosed()
      .subscribe((result: boolean) => {
        if (result) {
          this.resource.delete(id)
            .subscribe(() => this.model.splice(index, 1));
        }
      });
  }
}
