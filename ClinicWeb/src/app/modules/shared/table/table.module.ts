import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from 'src/app/components/shared/table/table.component';
import { PagingComponent } from 'src/app/components/shared/table/paging/paging.component';

@NgModule({
  declarations: [
    TableComponent,
    PagingComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    TableComponent,
    PagingComponent
  ]
})
export class TableModule { }
