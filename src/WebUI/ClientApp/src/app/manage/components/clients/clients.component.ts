import { Component, Input, OnInit } from "@angular/core";
import { ActionInfo } from "src/app/commons/classes/action-info";

@Component({
    selector: 'app-clients-component',
    templateUrl: './clients.component.html',
    styleUrls: ['./clients.component.scss']
})
export class ClientsComponent implements OnInit {

    actions: ActionInfo[] = [] 

    ngOnInit(): void {
        this.loadActions()
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