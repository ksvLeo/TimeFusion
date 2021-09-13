import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { ActionInfo } from '../../classes/action-info';
import { FieldInfo } from '../../classes/field-info';
import { PaginatedList } from '../../classes/paginated-list';
import { PagingParameters } from '../../classes/paging-parameters';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent implements OnInit {

  loading: boolean = true;
  _paginatedList: PaginatedList<any>;
  _totalPages: number[] = [];
  @Input() paginatedList: Observable<PaginatedList<any>>;
  @Input() configurationInfo: FieldInfo[] = [];
  @Output() paginate: EventEmitter<PagingParameters> = new EventEmitter<PagingParameters>();
  @Input() actionList: ActionInfo[] = [];
  @Input() allowsActions: boolean;
  @Output() itemClickEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor() { }

  ngOnInit(){
    this.paginatedList.subscribe(val => {
      this._paginatedList = val;
      this._totalPages = Array(val.totalPages).fill(1).map((x,i)=>i+1);
      this.loading = false;
      console.log("Finish loading");
    })
  }

  sortByColumn(info: FieldInfo){
    console.log("Sorting by: " + info.property);
    let pagingParam = new PagingParameters(1, 10, 1, info.property);
    this.loading = true;
    console.log("Loading");
    this.paginate.emit(pagingParam);
  }
  
  onItemClick(item: any){
    console.log("before")
    this.itemClickEvent.emit(item);
  }

}
