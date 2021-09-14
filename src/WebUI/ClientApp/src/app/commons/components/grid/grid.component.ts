import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActionInfo } from '../../classes/action-info';
import { FieldInfo, GridConfiguration } from '../../classes/grid-configuration';
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
      this._totalPages = Array(value.totalPages).fill(1).map((x,i)=>i+1);
      this._pageIndex = value.pageIndex;
      this.loading = false;
      console.log("Finish loading");
    }
  };
  _orderField: string;
  _pageIndex: number;
  _order: number = 1;
  _pageSize: number;
  _fieldsInfo: FieldInfo[];
  _itemsPerPage: number[];
  @Input() 
  set gridConfiguration(val: GridConfiguration){
    if (val) {
      this._fieldsInfo = val.FieldInfo;
      this._itemsPerPage = val.ItemsPerPage;
      this._pageSize = val.ItemsPerPage[0];
    }
  };
  @Output() paginate: EventEmitter<PagingParameters> = new EventEmitter<PagingParameters>();
  @Input() actionList: ActionInfo[] = [];
  @Input() allowsActions: boolean = false;
  @Output() itemClickEvent: EventEmitter<any> = new EventEmitter<any>();
  @Input() allowsClick: boolean = false;

  constructor() {}

  emitPaginate(){
    let pagingParam = new PagingParameters(this._pageIndex, this._pageSize, this._order, this._orderField);
    this.loading = true;
    console.log("Loading");
    this.paginate.emit(pagingParam);
  }
  
  onItemClick(item: any){
    this.itemClickEvent.emit(item);
  }

  sortByColumn(info: FieldInfo){
    if(this._orderField == info.property){
      this._order = this._order == 1 ? 2 : 1;
    }else{
      this._orderField = info.property;
      this._order = 1;
    }
    this.emitPaginate();
  }

  changePage(page: number){
    this._pageIndex = page;
    this.emitPaginate();
  }

  previousPage(){
    if (this._paginatedList.hasPreviousPage) {
      this.changePage(this._pageIndex - 1);
    }
  }

  nextPage(){
    if (this._paginatedList.hasNextPage) {
      this.changePage(this._pageIndex + 1);
    }
  }

  itemPerPage(value){
    this._pageSize = value;
    this._pageIndex = 1;
    this.emitPaginate();
  }

}
