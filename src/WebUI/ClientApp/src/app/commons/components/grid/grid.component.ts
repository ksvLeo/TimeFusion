import { Component, Input, OnInit } from '@angular/core';
import { FieldInfo } from '../../classes/field-info';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  @Input() paginatedList: any;
  @Input() configurationInfo: FieldInfo[];

  constructor() { }

  ngOnInit(): void {
  }

}
