import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ClientClient, ClientDto, CreateClientCommand, CurrencyDto, CurrencyReferenceClient, ContactDto, UpdateClientCommand } from "src/app/web-api-client";

@Component({
    selector: 'app-create-edit-client-component',
    templateUrl: './create-edit-client.component.html',
    styleUrls: ['./create-edit-client.component.scss']
})
export class CreateEditClientCompontent implements OnInit{

    // Form
    clientForm: FormGroup;
    contactForm: FormGroup;
    createContact: boolean = false;
    isClientFormValid: boolean = false;
    isContactFormValid: boolean = false;
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

        this.contactForm = this.fb.group({
            title : [ "" , [Validators.minLength(4)]],
            name: [ "", [Validators.minLength(3), Validators.required]],
            email: [ "", [Validators.email]],
            phone: [ "", []]
        });

        this.contactForm.valueChanges.subscribe(changes =>{
            this.contactFromChanges(changes);
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

    contactFromChanges($values){
        this.isContactFormValid = this.contactForm.valid;
        this.validForm();
    }

    onCreateContact(){
        if(this.createContact){
            this.createContact = false;
            this.contactForm.reset();
            this.validForm();
            return;
        }

        this.createContact = true;
        this.validForm();
    }

    validForm(){
        if(this.client){
            this.isClientFormValid = true;
        }
        if(this.isClientFormValid && !this.createContact && !this.userExist){
            this.isFormsValid = false;
            return;
        }
        if(this.isClientFormValid && this.isContactFormValid && this.createContact){
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
            var referrer = this.mapReferrer(this.contactForm);
        }
        let client  = this.mapClient(this.clientForm, referrer);

        if(this.clientEdit){
            client.id = this.client.id;
            client.contactList = this.client.contactList;
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
            if(this.createContact){
                this.contactForm.reset();
            }
            this.router.navigate(['manage/clients']);
        }, err => {});

    }
    
    mapReferrer(contactForm: FormGroup): ContactDto {
        return new ContactDto({
            name : contactForm.get('name').value,
            title: contactForm.get('title').value,
            email: contactForm.get('email').value,
            phoneNumber: contactForm.get('phone').value
        });
    }


    mapClient(clientForm: FormGroup, contact: ContactDto) : ClientDto{
        let contacts : ContactDto[] = [];
        contacts.push(contact);
        let currency = this.currencies.find(c => c.id == clientForm.get('currency').value);
        let client =  new ClientDto({
            name : clientForm.get('name').value,
            address : clientForm.get('address').value,
            currency: currency,
            contactList: contacts,
        });

        return client;
    }
}