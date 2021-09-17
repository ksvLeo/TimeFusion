import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SelectInfo } from "src/app/shared/interfaces/selectInfo";
import { ClientClient, ClientDto, ContactClient, ContactDto, CreateClientCommand, CreateClientResult, CreateContactCommand, CreateContactResult, CreateContactResultDto, CurrencyDto, PaginatedListOfClientDto, UpdateContactCommand, UpdateContactResult } from "src/app/web-api-client";
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject } from "rxjs";
import { ModeParameter } from "src/app/shared/enums/modeParameter";

@Component({
    selector: 'app-create-edit-contact-component',
    templateUrl: './create-edit-contact.component.html',
    styleUrls: ['./create-edit-contact.component.scss']
})

export class CreateEditContactComponent implements OnInit {

    // Form
    contactForm: FormGroup;
    isFormValid: boolean = false;
    isNotSelectedClient: boolean;

    // Data
    clients: ClientDto[] = [];
    nameExists: boolean = false;
    clientId: number;

    selectInfo : SelectInfo = {
        buttonCreateName: "Create Client",
        canCreate: true,
        label: 'Client',
        required: true
    }
    //Edit
    contactEdit: boolean = false;
    contact: ContactDto;
    selectedItem: Subject<number> = new Subject<number>();

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private contactClient: ContactClient,
        private activeRoute: ActivatedRoute,
        private router: Router,
        private toastrService: ToastrService 
    ){ 
    }
    
    ngOnInit(){
        
        this.contactForm = this.fb.group({
            title: ["", []],
            name: [ "", [Validators.required]],
            email: [ "", [Validators.email]],
            phone: ["", []],
        });
        
        this.contactForm.valueChanges.subscribe(changes => {
            this.contactFormChanges(changes);
        });

        this.getClientsList().then(() => { this.processRouteParameter() });
    }

    processRouteParameter(){
        var urlParams = this.activeRoute.snapshot.params
        switch (Number(urlParams['mode'])){
            case ModeParameter.Create:
                if (urlParams['id'] != null){
                    this.clientClient.getClient(Number(urlParams['id'])).subscribe(res =>{
                        this.selectedItem.next(res.id);
                    });
                }
                break;
            case ModeParameter.Edit:
                if (urlParams['contactId'] != null){
                    this.getContactForEdit(Number(urlParams['id']), Number(urlParams['contactId']));
                } 
                break;
            default:
                this.router.navigate['/management/clients/']
                break;
        }
    }

    getClientsList(): Promise<any>{
        return this.clientClient.get(0,0,1,null,null).toPromise().then(res => this.clients = res.items);
    }

    contactFormChanges($values){
        this.contactClient.validateName(this.contact? this.contact.id : null, this.clientId, $values.name).subscribe(res => { 
            this.nameExists = res;
            this.validatorsContact();
        }, err => {});
    }

    validatorsContact(){
         if(!this.clientId){
            this.isNotSelectedClient = true;
            return;
        }
        
        if(this.contactEdit){
            this.isFormValid = false;
            this.nameExists = false;
            if(this.contactEdit){
                this.isFormValid = true;
            }
            return;
        }
        
        if(this.contactEdit &&  this.contact.name == this.contactForm.get('name').value){
            this.isFormValid = true;
            return;
        }
        
        debugger;
        this.isFormValid = this.contactForm.valid && !this.nameExists;
    }

    getContactForEdit(id: number, contactId: number){
        this.clientId = id;
        this.contactClient.getContact(contactId).subscribe(res =>{
            this.contactEdit = true;
            debugger;
            this.contact = res;
            this.selectedItem.next(this.contact.clientId);
            this.contactForm.setValue({
                title: [ this.contact.title],
                name: [ this.contact.name],
                email: [ this.contact.email],
                phone: [ this.contact.phoneNumber]
            });
            this.contactForm.controls.title.setErrors(null);
            this.contactForm.controls.email.setErrors(null);
        });
    }

    saveContact(){
        this.isFormValid = false;
        let contact = this.mapContact(this.contactForm);

        if(this.contactEdit){
            contact.phoneNumber = contact.phoneNumber.toString();
            contact.name = contact.name.toString()
            contact.title = contact.title.toString();
            contact.id = this.contact.id;
            contact.email = contact.email.toString();
            this.contactClient.updateContact(new UpdateContactCommand({ clientId: this.clientId, newContact: contact})).subscribe(res => {
                switch (res) {
                    case UpdateContactResult.Success:
                        this.contactForm.reset();
                        this.toastrService.success("The contact has been update successfully.");
                        this.router.navigate(['/manage/client/', this.clientId]);
                        break;
                    case UpdateContactResult.EmptyName:
                        this.toastrService.warning("Name field can't be null.");
                        break;
                    case UpdateContactResult.Error_NameExists:
                        this.toastrService.warning("Already exists a client with name selected.");
                        break;
                
                    default:
                        break;
                }
            }, err => {
                this.toastrService.error("An error occurred while updating the contact.");
            });
            return;
        }
        this.contactClient.createContact(new CreateContactCommand({ clientId: this.clientId, contact: contact })).subscribe(res => {
            switch (res.result) {
                case CreateContactResult.Success:
                    this.contactForm.reset();
                    this.toastrService.success("The contact has been created successfully.");
                    this.router.navigate(['/manage/clients']);
                    break;
                case CreateContactResult.EmptyName:
                    this.toastrService.warning("Name field can't be null.");
                    break;
                case CreateContactResult.Error_NameExists:
                    this.toastrService.warning("Already exists a contact with name selected.");
                    break;
            
                default:
                    break;
            }
        }, err => {
            this.toastrService.error("An error occurred while creating the contact.");
        });
    }

    mapContact(contactForm: FormGroup): ContactDto{
        return new ContactDto({
            name: contactForm.get('name').value,
            email: contactForm.get('email').value,
            title: contactForm.get('title').value,
            phoneNumber: contactForm.get('phone').value
        });
    }

    processClientId(client: ClientDto){
        if(!client){
            this.isFormValid = false;
            this.clientId = null;
            this.nameExists = false;
            this.isNotSelectedClient = false;
            return;
        }
        this.clientId = client.id;
        this.isNotSelectedClient = false;
    }
    
    // Testing
    // Test integration select with createEntity
    createNewClient(newClient : ClientDto){
        debugger;
        let client: ClientDto = new ClientDto({
            name : newClient.name,
            address: null,
            currency: new CurrencyDto({id:1}),
            contactList: null
        });
        this.clientClient.createClient(new CreateClientCommand({newClient: client})).subscribe(res => {
            switch(res.result){
                case CreateClientResult.Success:
                    this.toastrService.success("The client has been created successfully.");
                    this.getClientsList();
                    this.clientId = res.id;
                    break;
                case CreateClientResult.Error_NameExists:
                    this.toastrService.warning("Already exists a client with name selected.");
                    break;
                default:
                    this.toastrService.error("An error occurred while creating the client.");
                    break;       
            }
        }, err => {});
    }
    // Testing

    onCancelClick() {
        if (this.contactEdit)
            this.router.navigate(['manage/client/', this.contact.clientId])
        else
            this.router.navigate(['manage/clients'])
    }
}
