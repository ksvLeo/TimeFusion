import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ActionInfo } from '../commons/classes/action-info';
import { FieldFormat, FieldInfo, GridConfiguration } from '../commons/classes/grid-configuration';
import { PaginatedList } from '../commons/classes/paginated-list';
import { PagingParameters } from '../commons/classes/paging-parameters';
import { ClientClient } from '../web-api-client';

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.css']
})
export class TeamComponent implements OnInit {

  closeModal: any;
  paginatedList: PaginatedList<string>;
  fieldInfo: FieldInfo[] = [];
  gridConfiguration: GridConfiguration;
  pagingParams: PagingParameters = new PagingParameters(1, 10, 1, "name");
  filter = new FormControl('');
  actionList: ActionInfo[] = []

  constructor(
    private clientClient: ClientClient,
    private router: Router,
    private modalService: NgbModal) { }

  ngOnInit(): void {
  }

  configurationGrid(){
    this.fieldInfo = [
        new FieldInfo("Name", "name", FieldFormat.text, true),
        new FieldInfo("Address", "address", FieldFormat.text, true),
        new FieldInfo("Status", "status", FieldFormat.enum, true)
    ];
    this.gridConfiguration = new GridConfiguration(this.fieldInfo, [5, 10, 20]);
  }
  onPaginate(pagingParameter: PagingParameters){
    this.pagingParams = pagingParameter;
    
  }

  handleAddCollaborator(){
    
  }

  changeFilter(){
    this.pagingParams.PageNumber = 1;
    setTimeout(() => {
        
    }, 750);
    
  }

}
