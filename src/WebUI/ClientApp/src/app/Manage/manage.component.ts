import { Component } from "@angular/core";
import { NavMenuItemInterface } from "../interfaces/navMenuItem";

@Component({
    selector: 'app-manage-component',
    templateUrl: './manage.component.html',
    styleUrls: ['./manage.component.scss']
})
export class ManageComponent {

    flag: boolean = false;

    test: string = "hola"    ;

    manageMenu: NavMenuItemInterface[] = [
        {
            description: "Clients",
            url: "./clients"
        },
        {
            description: "Tasks",
            url: ""
        },
        {
            description: "Expense Categories",
            url: ""
        },
        {
            description: "Roles",
            url: ""
        }
    ];

    active(){
        this.flag = this.flag;
    }
}


