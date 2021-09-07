import { Component } from "@angular/core";

@Component({
    selector: 'app-manage-component',
    templateUrl: './manage.component.html',
    styleUrls: ['./manage.component.scss']
})
export class ManageComponent {

    layout = [
        {
            name: "Clients",
            url: "/"
        },
        {
            name: "Tasks",
            url: "/"
        },
        {
            name: "Expense Categories",
            url: "/"
        },
        {
            name: "Roles",
            url: "/"
        }
    ];
}