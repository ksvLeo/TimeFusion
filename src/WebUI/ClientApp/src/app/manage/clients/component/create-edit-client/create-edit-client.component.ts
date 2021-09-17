import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ClientClient, ClientDto, CreateClientCommand, CurrencyDto, CurrencyReferenceClient, ContactDto, UpdateClientCommand, CreateClientResult, UpdateClientResult } from "src/app/web-api-client";
import { ToastrService } from 'ngx-toastr';
import { ModeParameter } from "src/app/shared/enums/modeParameter";

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
    areFormsValid: boolean = false;

    // Data
    currencies: CurrencyDto[] = [];
    userExist: boolean;
    defaultCurrency: CurrencyDto;
    contact: ContactDto;

    //Edit
    clientEdit: boolean = false;
    client: ClientDto;

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private currencyClient: CurrencyReferenceClient,
        private activeRoute : ActivatedRoute,
        private router: Router,
        private toastrService: ToastrService
    ){}
    
    ngOnInit(){
        this.getCurrencies();
        
        var urlParams = this.activeRoute.snapshot.params
        switch (Number(urlParams['mode'])) {
            case ModeParameter.Create:
                break;
            case ModeParameter.Edit:
                this.getClientForEdit();
                break;
            default:
                this.router.navigate['/manage/clients/']
                break;
        }
        
        this.clientForm = this.fb.group({
            name: ["",[Validators.required]],
            address: ["", [Validators.minLength(3)]],
            currency: ["", [Validators.required]]
        });
        
    
        this.clientForm.valueChanges.subscribe(changes => {
            this.clientFormChanges(changes);
        });

        this.contactForm = this.fb.group({
            title : [ "" , [Validators.minLength(3)]],
            name: [ "", [Validators.required]],
            email: [ "", [Validators.email]],
            phone: [ "", []]
        });

        this.contactForm.valueChanges.subscribe(changes =>{
            this.contactFormChanges(changes);
        });
    }

    getClientForEdit(){
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
                    this.clientForm.controls.address.setErrors(null);
                    this.areFormsValid = true;
                }
            });
        });
    }

    getCurrencies(){
        this.currencyClient.getCurrencyReferences().subscribe(res =>{
            this.currencies = res;
            if(!this.clientEdit){
                this.defaultCurrency = this.currencies.find(c => c.id == 1);
                this.clientForm.controls.currency.setValue(this.defaultCurrency.id);
            }
        }, err => {});
    }

    clientFormChanges($values){
        if(this.userExist){
            this.userExist = false;
        }
        if(this.clientEdit && $values.name == this.client.name){
            this.areFormsValid = this.clientForm.valid;
            return;
        }
        
        if($values && $values.name && $values.name.length > 0){
            this.isClientFormValid = this.clientForm.valid && !this.clientExistByName();
            this.areFormsValid = this.isClientFormValid && this.validForm();
        }
    }

    contactFormChanges($values){
        this.isContactFormValid = this.contactForm.valid;
        this.areFormsValid = this.isContactFormValid && this.validForm();
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

    clientExistByName(): boolean{
        this.clientClient.validateClientNameExistQuery(this.clientForm.get('name').value).subscribe(res => {
            this.userExist = res;
            this.areFormsValid = this.validForm();
            return this.userExist;
        }, err => {});
        return this.userExist;
    }

    validForm(): boolean{
        if(this.isClientFormValid &&!this.createContact && !this.userExist){
            this.areFormsValid = true;
            return true;
        }
        if(this.isClientFormValid && this.isContactFormValid && this.createContact && !this.userExist){
            this.areFormsValid = true;
            return true;
        }
        return false;    
    }

    saveClient(){
        if(!this.areFormsValid){
            this.areFormsValid = true;
        }

        if(!this.clientEdit && this.createContact){
            this.contact = this.mapContact(this.contactForm);
        }else{
            this.contact = null;
        }
        let client  = this.mapClient(this.clientForm, this.contact);

        if(this.clientEdit){
            client.id = this.client.id;
            client.contactList = this.client.contactList;
            client.name = client.name.toString();
            client.address = client.address.toString();
            let common = new UpdateClientCommand({client: client})
            this.clientClient.updateClient(common).subscribe(res => {
                
                switch(res){
                    case UpdateClientResult.Success:
                        this.clientForm.reset();
                        this.toastrService.success("The client was successfully updated.");
                        history.back();
                        // this.router.navigate(['manage/clients']);
                        break;
                    case UpdateClientResult.EmptyName:
                        this.toastrService.warning("Name field can't be null.");
                        break;
                    case UpdateClientResult.Error_NameExists:
                        this.toastrService.warning("Already exists a client with name selected.");
                        break;
                    default:
                           this.toastrService.error("An error occurred while updating the client.") ;
                        break;
                }

            }, err => {
                this.toastrService.error("An error occurred while updating the client.");
            });
            return;
        }

        this.clientClient.createClient(new CreateClientCommand({newClient: client})).subscribe(res =>{
            switch (res.result) {
                case CreateClientResult.Success:
                    this.toastrService.success("The client has been created successfully.");
                    this.clientForm.reset();
                    if(this.createContact){
                        this.contactForm.reset();
                    }
                    this.router.navigate(['manage/clients']);
                    break;
                case CreateClientResult.Error_NameExists:
                    this.toastrService.warning("Already exists a client with name selected.");
                    break;
                default:
                    this.toastrService.error("An error occurred while creating the client.");
                    break;
            }
        }, err => {
            this.toastrService.error("An error occurred while creating the client.");
        });

    }
    
    mapContact(contactForm: FormGroup): ContactDto {
        return new ContactDto({
            name : contactForm.get('name').value,
            title: contactForm.get('title').value,
            email: contactForm.get('email').value,
            phoneNumber: contactForm.get('phone').value
        });
    }


    mapClient(clientForm: FormGroup, contact: ContactDto) : ClientDto{
        let contacts : ContactDto[] = [];
        if(this.contact != null){
            contacts.push(contact);
        }else{
            contacts = null;
        }
        let currency = this.currencies.find(c => c.id == clientForm.get('currency').value);
        let client =  new ClientDto({
            name : clientForm.get('name').value,
            address : clientForm.get('address').value,
            currency: currency,
            contactList: contacts,
        });

        return client;
    }

    fuckGoBack() {
        history.back();
    }
}