import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AbNomenclatureSelectComponent } from './components/ab-nomenclature-select.component';
import { AbNomenclatureTemplatePipe } from './components/ab-nomenclature-template.pipe';

@NgModule({
  declarations: [
    AbNomenclatureSelectComponent,
    AbNomenclatureTemplatePipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule
  ],
  exports: [
    AbNomenclatureSelectComponent
  ]
})
export class NomenclatureSelectModule { }
