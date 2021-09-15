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

    isNotElement: boolean;
    nameForFind: string = "";
    nameAlreadyExist: boolean;
    item : any;
    isSelectOption: boolean;
    isNewEntity: boolean = false;

    private evetsSubscription: Subscription;

    @Input() items: any[];
    @Input() selectInfo: SelectInfo;
    @Input() displayProperty: string;
    @Input() idProperty: string;

    @Output() selected = new EventEmitter<any>();
    @Output() clientName = new EventEmitter<string>();
    @Output() newEntity = new EventEmitter<ClientDto>();

    constructor(){
    }

    ngOnInit(){
    }

    create(){
        // Create new entity
        let newEntity: any;
        newEntity[this.displayProperty] = this.nameForFind
        this.isNewEntity = true;
        this.isSelectOption = true;
        this.newEntity.emit(newEntity);
    }

    filterItems(){
        if(this.isNewEntity){
            return;
        }

        this.nameAlreadyExist = false;

        let results = this.items.filter(c => {
          return c[this.displayProperty].toLowerCase().includes(this.nameForFind.toLowerCase());
        });

        if(results.length > 0){
            this.isNotElement = false;
            return results;
        }

        this.isNotElement = true;
        return [];
    }

    findNameClient(){
    }

    cancelSelection(){
        this.isNewEntity = false;
        this.isNotElement = false;
        this.isSelectOption = false;
        this.nameAlreadyExist = false;
        this.nameForFind = "";
    }

    selectItem(item: any)
    {
      console.log("selected: " + item[this.displayProperty]);
      this.item = item;
      this.selected.emit(this.item);
    }
};
