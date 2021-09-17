import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { FormControl } from "@angular/forms";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { filter } from "rxjs/operators";
import { ActionInfo } from "src/app/commons/classes/action-info";
import { ContactManagementUrlParams } from "src/app/commons/classes/contactManagementUrlParams";
import { FieldFormat, FieldInfo, GridConfiguration } from "src/app/commons/classes/grid-configuration";
import { ModalInfo } from "src/app/commons/classes/modal-info";
import { PaginatedList } from "src/app/commons/classes/paginated-list";
import { PagingParameters } from "src/app/commons/classes/paging-parameters";
import { GenericModalComponent } from "src/app/commons/components/generic-modal/generic-modal.component";
import { ClientClient, ClientDto, ClientStatus } from "src/app/web-api-client";

@Component({
    selector: 'app-clients-component',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit {

    closeModal: any;
    paginatedList: PaginatedList<ClientDto>;
    fieldInfo: FieldInfo[] = [];
    gridConfiguration: GridConfiguration;
    pagingParams: PagingParameters = new PagingParameters(1, 10, 1, "name");
    filter = new FormControl('');
    actionList: ActionInfo[] = []

    constructor(
        private clientClient: ClientClient,
        private router: Router,
        private modalService: NgbModal,
        private route: ActivatedRoute
    ){}

    ngOnInit(): void {
        this.configurationGrid();
        this.getClients();
        this.loadActions()
    }

    configurationGrid(){
        this.fieldInfo = [
            new FieldInfo("Name", "name", FieldFormat.text, true),
            new FieldInfo("Status", "status", FieldFormat.enum, true)
        ];
        this.gridConfiguration = new GridConfiguration(this.fieldInfo, [10, 20, 30]);
    }

    getClients(pageNumber: number = 1, pageSize: number = 10, order: number = 1, orderField: string = "name", filter: string = ""): void{
        this.clientClient.get(pageNumber, pageSize, order, orderField, filter).subscribe(res => this.paginatedList = res);
    }

    onPaginate(pagingParameter: PagingParameters){
        this.pagingParams = pagingParameter;
        this.getClients(pagingParameter.PageNumber, pagingParameter.PageSize, pagingParameter.Order, pagingParameter.OrderField, this.filter.value);
    }

    changeFilter(){
        this.pagingParams.PageNumber = 1;
        this.getClients(this.pagingParams.PageNumber, this.pagingParams.PageSize, this.pagingParams.Order, this.pagingParams.OrderField, this.filter.value);
    }
        
    onAddContact(item: any) {
        let urlParams = new ContactManagementUrlParams()
        urlParams.mode = 1;
        if(item != null)
        urlParams.id = item.id.toString()
        this.router.navigate(['/manage/client/contact', urlParams]); 
    }

    onAddClient(item: any){
        let urlParams = new ContactManagementUrlParams()
        urlParams.mode = 1;
        if(item != null)
        urlParams.id = item.id.toString()
        this.router.navigate(['/manage/client', urlParams])
    }
        
    onEditContacts(item: any) {
        // Not implemented in list client
        let urlParams = new ContactManagementUrlParams()
        urlParams.mode = 2;
        if(item != null)
        urlParams.id = item.id.toString()
        this.router.navigate(['/manage/contact', urlParams]); 
    }

    onEditClient(item: any) {
        let urlParams = new ContactManagementUrlParams()
        urlParams.mode = 2;
        if(item != null)
        urlParams.id = item.id.toString()
        this.router.navigate(['/manage/client', urlParams]); 
    }

    onFlagClient(item: ClientDto) {
        let modalInfo = new ModalInfo()
        if(item.status == ClientStatus.Active) {
            modalInfo.title = "Deactivate Client?"
            modalInfo.message = "Are you ready to finish your work with " + item.name + "?"  
        } else {
            modalInfo.title = "Reactivate Client?"
            modalInfo.message = "Are you ready to work with " + item.name + " again?"
        }
        this.openModal(modalInfo).then(input => {
            if(input == "accept") {
                if (item.status == ClientStatus.Active) {
                    this.clientClient.deleteClient(item.id).subscribe(response => {
                        this.getClients()
                    })
                    
                } else {
                    this.clientClient.reactivateClient(item.id).subscribe(response => {
                        this.getClients()
                    })
                }
            }
        })
    }
        
    onViewProjects(item: any) {

    }


    loadActions() {
        let action: ActionInfo
        action = new ActionInfo();
        action.label = "Add Contact";
        action.enable = true;
        action.event.subscribe(item => this.onAddContact(item));
        this.actionList.push(action)
        
        action = new ActionInfo();
        action.label = "Edit Client";
        action.enable = true;
        action.event.subscribe(item => this.onEditClient(item));
        this.actionList.push(action);

        action = new ActionInfo();
        action.label = "Flag Client";
        action.enable = true;
        action.event.subscribe(item => this.onFlagClient(item));
        this.actionList.push(action);

        action = new ActionInfo();
        action.label = "View Projects";
        action.enable = true;
        action.event.subscribe(item => this.onViewProjects(item));
        this.actionList.push(action);
    }

    onClientDetailClick(item: any) {
        this.router.navigate(['/manage/client', item.id])
    }

    openModal(modalInfo: ModalInfo): Promise<string> {
        var promise = new Promise<string>((resolve) => {
          setTimeout(() => {
            var modalRef = this.modalService.open(GenericModalComponent,
                {
                  scrollable: true
  
                })
            modalRef.componentInstance.title = modalInfo.title;
            modalRef.componentInstance.message = modalInfo.message;
            modalRef.result.then((result:any) => {
            resolve(result)}, (reason:any) => {
            });
          }, 150);
      });
      return promise;
    }
}
