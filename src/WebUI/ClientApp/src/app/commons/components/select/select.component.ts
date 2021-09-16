import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { Observable, Subscription } from "rxjs";
import { SelectInfo } from "src/app/shared/interfaces/selectInfo";

@Component({
    selector: 'app-select-component',
    templateUrl: './select.component.html',
    styleUrls: ['./select.component.scss']
})

export class SelectComponent implements OnInit {

    isNotElement: boolean;
    nameForFind: string = "";
    nameAlreadyExists: boolean = false;
    item : any;
    isSelectOption: boolean;
    isNewEntity: boolean = false;
    showList: boolean = false;
    disabledCancel: boolean = false;

    private eventSubscription: Subscription;

    @Input() items: any[];
    @Input() selectInfo: SelectInfo;
    @Input() displayProperty: string;
    @Input() idProperty: string;
    @Input() itemSelected: Observable<number>;

    @Output() selected = new EventEmitter<any>();
    @Output() clientName = new EventEmitter<string>();
    @Output() newEntity = new EventEmitter<any>();

    constructor(){
    }
    
    ngOnInit(){
        this.eventSubscription = this.itemSelected.subscribe(res => this.itemAlredySelected(res));
    }

    itemAlredySelected(res: number){
        this.disabledCancel = true;
        let item = this.items.find(c => c[this.idProperty] == res);
        this.selectItem(item);
    }

    create(){
        // Create new entity
        let newEntity: any = {};
        newEntity[this.displayProperty] = this.nameForFind;

        this.isNewEntity = true;
        this.isSelectOption = true;
        this.newEntity.emit(newEntity);
    }

    filterItems(){
        if(this.isNewEntity){
            return;
        }

        this.nameAlreadyExists = false;

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
        let t = this.items.find(c => c[this.displayProperty].toLowerCase() == this.nameForFind.toLocaleLowerCase());
        if(t){
            this.nameAlreadyExists = true;
            this.showList = false;
            return;
        }else{
            this.nameAlreadyExists = false;
        }

        if(this.items.filter(c => c[this.displayProperty].toLowerCase().includes(this.nameForFind.toLowerCase())).length > 0){
            this.isNotElement = true;
        }else{
            this.isNotElement = false;
            this.showList = false
        }
    }

    cancelSelection(){
        this.isNewEntity = false;
        this.isNotElement = false;
        this.isSelectOption = false;
        this.nameAlreadyExists = false;
        this.nameForFind = "";
    }

    selectItem(item: any)
    {
        this.showList = false;
        this.isSelectOption = true;
        this.item = item;
        this.nameForFind = item[this.displayProperty];
        this.selected.emit(this.item);
    }

    onShowList(){
        if(this.showList){
            this.showList = false;
            return;
        }

        this.showList = true;
    }
};
