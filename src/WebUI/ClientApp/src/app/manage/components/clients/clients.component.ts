import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ActionInfo } from "src/app/commons/classes/action-info";
import { ContactManagementUrlParams } from "src/app/commons/classes/contactManagementUrlParams";
import { FieldInfo, GridConfiguration } from "src/app/commons/classes/grid-configuration";
import { ModalInfo } from "src/app/commons/classes/modal-info";
import { PaginatedList } from "src/app/commons/classes/paginated-list";
import { PagingParameters } from "src/app/commons/classes/paging-parameters";
import { ClientClient, ClientDto } from "src/app/web-api-client";
import { GenericModalComponent } from "../../../commons/components/generic-modal/generic-modal.component";

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
    actionList: ActionInfo[] = []

    constructor(private clientClient: ClientClient,
                private router: Router,
                private modalService: NgbModal){}

    ngOnInit(): void {
        this.configurationGrid();
        this.getClients();
        this.loadActions()
    }

    configurationGrid(){
        this.fieldInfo = [
            new FieldInfo("Name", "name", "string", true),
            new FieldInfo("Status", "active", "string", true)
        ];
        this.gridConfiguration = new GridConfiguration(this.fieldInfo, [1, 2, 10]);
    }

    getClients(pageNumber: number = 1, pageSize: number = 1, order: number = 1, orderField: string = "name"): void{
        this.clientClient.get(pageNumber, pageSize, order, orderField).subscribe(res => this.paginatedList = res);
    }

    onPaginate(pagingParameter: PagingParameters){
        console.log(pagingParameter);
        this.getClients(pagingParameter.PageNumber, pagingParameter.PageSize, pagingParameter.Order, pagingParameter.OrderField);
    }
        
    onAddContact(item: any) {
        let urlParams = new ContactManagementUrlParams()
        urlParams.mode = "create"
        if(item != null)
        urlParams.id = item.id.toString()
        this.router.navigate(['/manage/clients/contact', urlParams]); 
    }
        
    onEditContacts(item: any) {

    }



    onEditClient(item: any) {
        this.router.navigate(['/manage/clients/edit', item.id])
    }

    onFlagClient(item: ClientDto) {
        let modalInfo = new ModalInfo()
        if(item.active) {
            modalInfo.title = "Deactivate Client?"
            modalInfo.message = "Are you ready to finish your work with " + item.name + "?"  
        } else {
            modalInfo.title = "Reactivate Client?"
            modalInfo.message = "Are you ready to work with " + item.name + " again?"
        }
        this.openModal(modalInfo).then(input => {
            if(input == "accept") {
                if (item.active) {
                    this.clientClient.deleteClient(item.id)
                    this.getClients()
                } else {
                    this.clientClient.reactivateClient(item.id)
                    this.getClients()
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
        action.label = "Edit Contacts";
        action.enable = true;
        action.event.subscribe(item => this.onEditContacts(item));
        this.actionList.push(action);

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
        this.router.navigate(['/manage/clients', item.id])
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
