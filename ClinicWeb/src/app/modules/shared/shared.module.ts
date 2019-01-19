import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ModalModule } from './modal/modal.module';
import { TableModule } from './table/table.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ModalModule,
    TableModule
  ],
  exports: [
    ModalModule,
    TableModule
  ]
})
export class SharedModule { }
