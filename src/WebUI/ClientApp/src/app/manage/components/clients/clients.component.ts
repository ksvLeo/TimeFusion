import { TransitiveCompileNgModuleMetadata } from "@angular/compiler";
import { Component, EventEmitter, OnInit, Output } from "@angular/core";
import { Router } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Observable } from "rxjs";
import { ActionInfo } from "src/app/commons/classes/action-info";
import { FieldFormat, FieldInfo, GridConfiguration } from "src/app/commons/classes/grid-configuration";
import { PaginatedList } from "src/app/commons/classes/paginated-list";
import { PagingParameters } from "src/app/commons/classes/paging-parameters";
import { ClientClient, ClientDto, ClientStatus } from "src/app/web-api-client";
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
            new FieldInfo("Name", "name", FieldFormat.text, true),
            new FieldInfo("Status", "status", FieldFormat.enum, true)
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
        this.router.navigate(['/manage/clients/contact/create', item.id])
    }
        
    onEditContacts(item: any) {

    }



    onEditClient(item: any) {
        this.router.navigate(['/manage/clients/edit', item.id])
    }

    onDeleteClient(item: any) {
        this.openModal(item)
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
        action.label = "Delete Client";
        action.enable = true;
        action.event.subscribe(item => this.onDeleteClient(item));
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

    openModal(item: any) {
        const modalRef = this.modalService.open(GenericModalComponent,
          {
            scrollable: true
          });
       
          modalRef.componentInstance.title = "Deactivate client?"
          modalRef.componentInstance.message = "Are you sure you want to deactivate " + item.Name + "?"
        modalRef.result.then((result:any) => {
          console.log(result);
        }, (reason:any) => {
        });
      }
}
