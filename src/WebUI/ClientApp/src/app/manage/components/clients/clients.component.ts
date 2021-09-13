import { Component, Input, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { ActionInfo } from "src/app/commons/classes/action-info";
import { FieldInfo } from "src/app/commons/classes/field-info";
import { PaginatedList } from "src/app/commons/classes/paginated-list";
import { PagingParameters } from "src/app/commons/classes/paging-parameters";
import { ClientClient, ClientDto, PaginatedListOfClientDto } from "src/app/web-api-client";

@Component({
    selector: 'app-clients-component',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit {

    paginatedList$: Observable<PaginatedList<ClientDto>>;
    tableConfig: FieldInfo[];
    actions: ActionInfo[] = []

    constructor(private clientClient: ClientClient){}

    ngOnInit(): void {
        this.configurationGrid();
        this.getClients();
        this.loadActions()
    }

    configurationGrid(){
        this.tableConfig = [
            new FieldInfo("Name", "name", "string", true),
            new FieldInfo("Address", "address", "string", true)
        ];
    }

    getClients(): void{
        this.paginatedList$ = this.clientClient.get(1, 5, 1, "Address");
    }

    onPaginate(pagingParameter: PagingParameters){
        console.log(pagingParameter);
        this.getClients();
    }
        
    onAddContact(item: any) {

    }
        
    onEditContacts(item: any) {

    }


    onEditClient(item: any) {

    }

    onDeleteClient(item: any) {

    }
        
    onViewProjects(item: any) {

    }


    loadActions() {
        let action: ActionInfo
        action = new ActionInfo();
        action.label = "Add Contact";
        action.enable = true;
        action.event.subscribe(item => this.onAddContact(item));
        this.actions.push(action)

        action = new ActionInfo();
        action.label = "Edit Contacts";
        action.enable = true;
        action.event.subscribe(item => this.onEditContacts(item));
        this.actions.push(action);

        action = new ActionInfo();
        action.label = "Edit Client";
        action.enable = true;
        action.event.subscribe(item => this.onEditClient(item));
        this.actions.push(action);

        action = new ActionInfo();
        action.label = "Delete Client";
        action.enable = true;
        action.event.subscribe(item => this.onDeleteClient(item));
        this.actions.push(action);

        action = new ActionInfo();
        action.label = "View Projects";
        action.enable = true;
        action.event.subscribe(item => this.onViewProjects(item));
        this.actions.push(action);
    }
}