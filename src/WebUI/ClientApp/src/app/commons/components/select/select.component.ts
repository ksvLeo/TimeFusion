import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { faSleigh } from "@fortawesome/free-solid-svg-icons";
import { Observable, Subscription } from "rxjs";
import { SelectInfo } from "src/app/interfaces/selectInfo";
import { ClientDto, CurrencyDto } from "src/app/web-api-client";

@Component({
    selector: 'app-select-component',
    templateUrl: './select.component.html',
    styleUrls: ['./select.component.scss']
})

export class SelectComponent implements OnInit {

    _items: ClientDto[] = null;
    isNotElement: boolean;
    nameForFind: string = "";
    nameAlreadyExist: boolean;
    _itemId : number;
    isSelectOption: boolean;
    isNewClient: boolean = false;

    private evetsSubscription: Subscription;

    @Input() items:  Observable<ClientDto[]>;
    @Input() selectInfo: SelectInfo;

    @Output() clientId = new EventEmitter<number>();
    @Output() clientName = new EventEmitter<string>();
    @Output() newClient = new EventEmitter<ClientDto>();

    constructor(){
    }

    ngOnInit(){
        this.evetsSubscription = this.items.subscribe((res) => this.getItems(res));
    }

    selectedClient($event){
        this._itemId = $event;
        this._items.forEach(i => {
            if(i.id == +this._itemId){
                this.nameForFind = i.name;
            }
        });
        this.isSelectOption = true;
        this.clientId.emit(this._itemId);
    }

    createClient(){
        // Create new 
        let newClient = new ClientDto({
            name : this.nameForFind,
            currency:  new CurrencyDto({id: 2})
        });
        this.isNewClient = true;
        this.isSelectOption = true;
        this.newClient.emit(newClient);
    }

    getItems(res: ClientDto[]){
        if(this.isNewClient){
            return;
        }

        this.nameAlreadyExist = false;

        res.forEach(c => {
            if(c.name.toLowerCase() == this.nameForFind.toLowerCase()){
                this.nameAlreadyExist = true;
                return;
            }
        });

        this._items = res;
        if(res.length > 0){
            this.isNotElement = false;
            return;
        }

        this.isNotElement = true;
    }

    findNameClient(){
        if(this.nameForFind.length < 3 || this.nameForFind == ""){
            this.isNotElement = false;
            this._items = [];
            return;
        }
        this.clientName.emit(this.nameForFind.trim());
    }

    cancelSelection(){
        this.isNewClient = false;
        this.isNotElement = false;
        this.isSelectOption = false;
        this.nameAlreadyExist = false;
        this.nameForFind = "";
        this._items = [];
    }
};