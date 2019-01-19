import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Column } from 'src/app/core/models/table/column.model';
import * as _ from 'lodash';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.styl']
})
export class TableComponent {
  @Input('columns') public columns: Column[];
  @Input('data') public data: any[];
  @Output('rowClicked') public onRowClicked = new EventEmitter<any>();
  public parser = _;

  public rowClicked(data: any): void {
    this.onRowClicked.emit(data);
  }
}
