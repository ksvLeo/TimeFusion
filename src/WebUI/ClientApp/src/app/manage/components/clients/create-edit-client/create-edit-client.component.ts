import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ClientClient, ClientDto, CreateClientCommand, CurrencyDto, CurrencyReferenceClient, ReferrerDto } from "src/app/web-api-client";

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
    isFormsValid: boolean = true;

    // Data
    currencies: CurrencyDto[] = [];
    userExist: boolean;
    defaultCurrency: CurrencyDto;

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private currencyClient: CurrencyReferenceClient
    ){}

    ngOnInit(){
        this.getCurrencies();
        
        this.clientForm = this.fb.group({
            name: ["",[Validators.minLength(3),Validators.required]],
            address: [ "",[Validators.minLength(4)]],
            currency: ["", [Validators.required]]
        });


        this.clientForm.valueChanges.subscribe(changes => {
            this.clientFormChanges(changes);
        });

        this.referrerForm = this.fb.group({
            title : [ , [Validators.minLength(4)]],
            name: [ , [Validators.minLength(3), Validators.required]],
            email: [ , [Validators.email]],
            phone: [ , []]
        });

        this.referrerForm.valueChanges.subscribe(changes =>{
            this.referrerFromChanges(changes);
        })
    }

    getCurrencies(): CurrencyDto[]{
        this.currencyClient.getCurrencyReferences().subscribe(res =>{
            this.currencies = res;
            this.defaultCurrency = this.currencies.find(c => c.id == 2);
            this.clientForm.patchValue({
                currency: [this.defaultCurrency.id]
            });
        }, err => {});

        return this.currencies;
    }

    clientFormChanges($values){
        if(this.userExist){
            this.userExist = false;
        }
    
        if((this.clientForm.get('name').value.trim()).length > 0){
            this.clientClient.getClientByName(this.clientForm.get('name').value.trim()).subscribe(res =>{
                if(res){
                    this.userExist = true;
                    return;
                }

                if(!this.userExist){
                    this.isClientFormValid = this.clientForm.valid;
                }
            });
        }else{
            this.userExist = false;
        }

        this.validForm();
    }

    referrerFromChanges($values){
        this.isReferrerFormValid = this.referrerForm.valid;
        this.validForm();
    }

    onCreateReferrer(){
        if(this.createReferrer){
            this.createReferrer = false;
            this.referrerForm.reset();
            this.validForm();
            return;
        }

        this.createReferrer = true;
        this.validForm();
    }

    validForm(){
        this.isFormsValid = true;
        if(this.isClientFormValid && !this.createReferrer && !this.userExist){
            this.isFormsValid = false;
            return;
        }
        if(this.isClientFormValid && this.isReferrerFormValid && this.createReferrer){
            this.isFormsValid = false;
            return;
        }

        this.isFormsValid = true;    
    }

    create(){
        let referrer = this.mapReferrer(this.referrerForm);
        let client  = this.mapClient(this.clientForm, referrer);
        this.clientClient.createClient(new CreateClientCommand({client : client})).subscribe(res =>{
            this.clientForm.reset();
            if(this.createReferrer){
                this.referrerForm.reset();
            }
        }, err => {});
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
            address : clientForm.get('address').value.trim(),
            currency: currency,
            referrer: referrers,
        });
    }
}