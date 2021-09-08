import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ClientClient, CurrencyDto, CurrencyReferenceClient } from "src/app/web-api-client";

@Component({
    selector: 'app-create-edit-client-component',
    templateUrl: './create-edit-client.component.html',
    styleUrls: ['./create-edit-client.component.scss']
})
export class CreateEditClientCompontent implements OnInit{

    // Form
    clientForm: FormGroup;
    referrerForm: FormGroup;
    createReferrer: boolean = false;
    isClientFormValid: boolean = false;
    isReferrerFormValid: boolean = false;

    // Data
    currencies: CurrencyDto[] = [];

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private currencyClient: CurrencyReferenceClient
    ){}

    ngOnInit(){
        this.getCurrencies();

        this.clientForm = this.fb.group({
            name: [ "",[Validators.minLength(3),Validators.required]],
            address: [ "",[Validators.minLength(4)]],
            currency: [ "", [Validators.required]]
        });

        this.clientForm.valueChanges.subscribe(changes => {
            this.clientFormChanges(changes);
        });

        this.referrerForm = this.fb.group({
            title : [ , [Validators.minLength(4), Validators.required]],
            name: [ , [Validators.minLength(3)]],
            email: [ , [Validators.email]],
            phone: [ , []]
        });
    }

    getCurrencies(){
        this.currencyClient.getCurrencyReferences().subscribe(res =>{
            this.currencies = res;
        }, err => {});
    }

    clientFormChanges($values){
        this.isClientFormValid = this.clientForm.valid;
    }

    referrerFromChanges($values){
        this.isReferrerFormValid = this.referrerForm.valid;
    }

    onCreateReferrerFrom(){
        if(this.referrerForm){
            this.createReferrer = false;
            return;
        }
    }

    create(){
        
    }
}