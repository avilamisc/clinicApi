import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ModalModule } from './modal/modal.module';
import { TableModule } from './table/table.module';
import { RaitingModule } from './raiting/star/star.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ModalModule,
    TableModule,
    RaitingModule
  ],
  exports: [
    ModalModule,
    TableModule,
    RaitingModule
  ]
})
export class SharedModule { }
