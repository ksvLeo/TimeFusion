import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ClientClient, ClientDto, CurrencyDto, CurrencyReferenceClient, ReferrerDto } from "src/app/web-api-client";

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

    onCreateReferrer(){
        if(this.createReferrer){
            this.createReferrer = false;
            return;
        }
        debugger;
        if(this.isClientFormValid){
            this.isClientFormValid = true;
        }
        this.createReferrer = true;
    }

    create(){
        let referrer = this.mapReferrer(this.referrerForm);
        let client  = this.mapClient(this.clientForm, referrer);

        console.log(client);
    }

    mapReferrer(referrerForm: FormGroup): ReferrerDto {
        return new ReferrerDto({
            name : referrerForm.get('name').value,
            title: referrerForm.get('title').value,
            email: referrerForm.get('email').value,
            phoneNumber: referrerForm.get('phone').value
        });
    }


    mapClient(clientForm: FormGroup, referrer: ReferrerDto) : ClientDto{
        let referrers : ReferrerDto[] = [];
        referrers.push(referrer);
        let currency = this.currencies.find(c => c.id == clientForm.get('currency').value);
        return  new ClientDto({
            name : clientForm.get('name').value,
            address : clientForm.get('address').value,
            currency: currency,
            referrer: referrers,
        });
    }

}