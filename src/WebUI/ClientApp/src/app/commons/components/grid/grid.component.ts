import { Component, Input, OnInit } from '@angular/core';
import { ActionInfo } from '../../classes/action-info';
import { FieldInfo } from '../../classes/field-info';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  @Input() paginatedList: any;
  @Input() configurationInfo: FieldInfo[] = [];
  @Input() actionsInfo: ActionInfo[] = [];
  @Input() allowsActions: boolean;

  constructor() { }

  ngOnInit(): void {
  }

}
