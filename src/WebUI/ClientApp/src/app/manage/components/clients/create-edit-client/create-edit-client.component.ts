import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ClientClient, ClientDto, CreateClientCommand, CurrencyDto, CurrencyReferenceClient, ReferrerDto, UpdateClientCommand } from "src/app/web-api-client";

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
    userExist: boolean = false;
    defaultCurrency: CurrencyDto;

    //Edit
    clientEdit: boolean = false;
    client: ClientDto;

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private currencyClient: CurrencyReferenceClient,
        private activeRoute : ActivatedRoute,
        private router: Router
    ){}

    ngOnInit(){
        
        this.clientForm = this.fb.group({
            name: ["",[Validators.minLength(3),Validators.required]],
            address: [""],
            currency: ["", [Validators.required]]
        });
        
            this.onClientEdit();
            
            this.getCurrencies();
    
        this.clientForm.valueChanges.subscribe(changes => {
            this.clientFormChanges(changes);
        });

        this.referrerForm = this.fb.group({
            title : [ "" , [Validators.minLength(4)]],
            name: [ "", [Validators.minLength(3), Validators.required]],
            email: [ "", [Validators.email]],
            phone: [ "", []]
        });

        this.referrerForm.valueChanges.subscribe(changes =>{
            this.referrerFromChanges(changes);
        })
    }

    onClientEdit(){
        this.activeRoute.params.subscribe(res => {
            if(res.id == null){
                return;
            }
            this.clientClient.getClient(res.id).subscribe(res => {
                this.client = res;
                if(this.client){
                    this.clientEdit = true;
                    this.clientForm.patchValue({
                        name : [this.client.name],
                        address: [this.client.address],
                        currency: [this.client.currency.id]
                    });
                    this.validForm();
                }
            });
        });
    }

    getCurrencies(){
        this.currencyClient.getCurrencyReferences().subscribe(res =>{
            this.currencies = res;
            if(!this.clientEdit){
                this.defaultCurrency = this.currencies.find(c => c.id == 2);
                this.clientForm.patchValue({    
                    currency: [this.defaultCurrency.id]
                });
            }
        }, err => {});
    }

    clientFormChanges($values){
        if(this.userExist){
            this.userExist = false;
        }

        if(this.clientEdit && this.client && this.clientForm.get('name').value == this.client.name){
            this.isClientFormValid = this.clientForm.valid;
            return;
        }
        
        if((this.clientForm.get('name').value.trim()).length > 0){
            this.clientClient.getClientByName(this.clientForm.get('name').value).subscribe(res =>{
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
        if(this.client){
            this.isClientFormValid = true;
        }
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

    saveClient(){
        if(!this.isFormsValid){
            this.isFormsValid = true;
        }

        if(!this.clientEdit){
            var referrer = this.mapReferrer(this.referrerForm);
        }
        let client  = this.mapClient(this.clientForm, referrer);

        if(this.clientEdit){
            client.id = this.client.id;
            client.referrer = this.client.referrer;
            client.name = client.name.toString();
            client.address = client.address.toString();
            let common = new UpdateClientCommand({client: client})
            this.clientClient.updateClient(common).subscribe(res =>{
                this.clientForm.reset();
                this.router.navigate(['manage/clients']);
            });
            return;
        }

        this.clientClient.createClient(new CreateClientCommand({ client : client })).subscribe(res =>{
            this.clientForm.reset();
            if(this.createReferrer){
                this.referrerForm.reset();
            }
            this.router.navigate(['manage/clients']);
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
        let client =  new ClientDto({
            name : clientForm.get('name').value,
            address : clientForm.get('address').value,
            currency: currency,
            referrer: referrers,
        });

        return client;
    }
}