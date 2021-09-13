import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { faSleigh } from "@fortawesome/free-solid-svg-icons";
import { Observable, Subscription } from "rxjs";
import { ClientDto, CurrencyDto } from "src/app/web-api-client";

@Component({
    selector: 'app-select-component',
    templateUrl: './select.component.html',
    styleUrls: ['./select.component.scss']
})

export class SelectComponent implements OnInit {

    _items: ClientDto[] = null;
    notElement: boolean;
    nameForFind: string;
    nameAlreadyExist: boolean;
    _itemId : number
    selectOption: boolean;
    addNewClient: boolean = false;
    // Test
    testing: string;
    // Test

    private evetsSubscription: Subscription;

    @Input() items:  Observable<ClientDto[]>;

    @Output() clientId = new EventEmitter<number>()    
    @Output() clientName = new EventEmitter<string>()
    @Output() newClient = new EventEmitter<ClientDto>()

    constructor(){
    }

    ngOnInit(){
        this.evetsSubscription = this.items.subscribe((res) => this.getItems(res));
    }

    test($event){
        this._itemId = $event;
        debugger;
        this._items.forEach(i => {
            if(i.id == +this._itemId){
                this.nameForFind = i.name;
            }
        });
        this.testing = this.nameForFind;
        this.nameForFind = null;
        this.selectOption = true;
        this.clientId.emit(this._itemId);
    }

    createClient(){
        // Create new 
        let newClient = new ClientDto({
            name : this.nameForFind,
            currency:  new CurrencyDto({id: 2})
        });
        this.addNewClient = true;
        this.newClient.emit(newClient);
    }

    getItems(res: ClientDto[]){
        this.nameAlreadyExist = false;
        res.forEach(c => {
            if(c.name.toLowerCase() == this.nameForFind.toLowerCase()){
                this.nameAlreadyExist = true;
                return;
            }
        });

        this._items = res;
        if(res.length > 0){
            this.notElement = false;
            return;
        }
        this.notElement = true;
    }

    findNameClient(){
        if(this.nameForFind.length == 0){
            this.notElement = false;
            this._items  = [];
        }
        console.log(this.nameForFind)
        this.clientName.emit(this.nameForFind.trim());
    }
};