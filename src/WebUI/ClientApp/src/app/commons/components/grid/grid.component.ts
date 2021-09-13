import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActionInfo } from '../../classes/action-info';
import { FieldInfo } from '../../classes/field-info';
import { PaginatedList } from '../../classes/paginated-list';
import { PagingParameters } from '../../classes/paging-parameters';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.css']
})
export class GridComponent {

  loading: boolean = true;
  _paginatedList: PaginatedList<any>;
  _totalPages: number[] = [];
  @Input() 
  set paginatedList(value: PaginatedList<any>){
    if(value){
      this._paginatedList = value;
      this.loading = false;
      this._totalPages = Array(value.totalPages).fill(1).map((x,i)=>i+1);
      this._pageIndex = value.pageIndex;
      console.log("Finish loading");
    }
  };
  _orderField: string;
  _pageIndex: number;
  _order: number = 1;
  _itemPerPage: number = 1;
  @Input() configurationInfo: FieldInfo[] = [];
  @Output() paginate: EventEmitter<PagingParameters> = new EventEmitter<PagingParameters>();
  // @Input() actionsInfo: ActionInfo[] = [];
  // @Input() allowsActions: boolean;

  constructor() {}

  callToParent(){
    let pagingParam = new PagingParameters(this._pageIndex, this._itemPerPage, this._order, this._orderField);
    this.loading = true;
    console.log("Loading");
    this.paginate.emit(pagingParam);
  }

  sortByColumn(info: FieldInfo){
    if(this._orderField == info.property){
      this._order = this._order == 1 ? 2 : 1;
    }else{
      this._orderField = info.property;
      this._order = 1;
    }
    this.callToParent();
  }

  onChangePage(page: number){
    this._pageIndex = page;
    this.callToParent();
  }

  movePage(i: number){
    this._pageIndex = this._pageIndex + i;
    this.callToParent();
  }

  itemPerPage(value){
    console.log(value);
    this._itemPerPage = value;
    this._pageIndex = 1;
    this.callToParent();
  }

}
