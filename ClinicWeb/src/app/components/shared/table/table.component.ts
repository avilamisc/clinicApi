import { Component, OnInit, Input } from '@angular/core';
import { Column } from 'src/app/core/models/table/column.model';
import * as _ from 'lodash';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.styl']
})
export class TableComponent implements OnInit {
  @Input() public columns: Column[];
  @Input() public data: any[];
  public parser = _;

  constructor() { }

  public ngOnInit() {
  }

}
