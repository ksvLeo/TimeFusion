import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SelectInfo } from "src/app/shared/interfaces/selectInfo";
import { ClientClient, ClientDto, ContactClient, ContactDto, CreateClientCommand, CreateClientResult, CreateContactCommand, PaginatedListOfClientDto, UpdateContactCommand } from "src/app/web-api-client";
import { ToastrService } from 'ngx-toastr';
import { Subject } from "rxjs";
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

    // Data
    clients: ClientDto[] = [];
    nameExist: boolean = false;
    clientId: number;

    selectInfo : SelectInfo = {
        buttonCreateName: "Create Client",
        isButtonCreated: false,
        label: 'Client',
        required: true
    }
    //Edit
    contactEdit: boolean = false;
    contact: ContactDto;
    slectedItem: Subject<number> = new Subject<number>();

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private contactClient: ContactClient,
        private activeRoute: ActivatedRoute,
        private router: Router,
        private toastrService: ToastrService 
    ){ 
        this.getClientsList().then(res => this.clients = res.items);
    }

    ngOnInit(){

        this.contactForm = this.fb.group({
            title: ["", [Validators.minLength(3)]],
            name: [ "", [Validators.minLength(3), Validators.required]],
            email: [ "", [Validators.email]],
            phone: ["", []],
        });

        this.contactForm.valueChanges.subscribe(changes => {
            this.contactFormChanges(changes);
        });
        var urlParams = this.activeRoute.snapshot.params
        switch (Number(urlParams['mode'])) {
            case ModeParameter.Create:
                this.getClientsList();
                if (urlParams['id'] != null){
                    this.clientClient.getClient(Number(urlParams['id'])).subscribe(res => {
                        this.slectedItem.next(res.id);
                    });
                }
                break;
            case ModeParameter.Edit:
                this.getClientsList();
                this.getContactForEdit();
                break;
            default:
                this.router.navigate['/management/clients/']
                break;
        }
    }

    contactFormChanges($values){
        if(!this.contactEdit && !$values || this.clientId == null || $values.name.length < 3){
            this.isFormValid = false;
            this.nameExist = false;
            if(this.contactEdit){
                this.isFormValid = true;
            }
            return;
        }

        if(this.contactEdit &&  this.contact.name == $values.name){
            this.isFormValid = true;
            return;
        }
        this.isFormValid = this.contactForm.valid && !this.contactNameExists(this.clientId,+$values.client, $values.name);
    }

    contactNameExists(contactId: number ,clientId: number, contactName: string): boolean{
        if(this.contactEdit){
            this.contactClient.validateName(contactId, clientId,contactName).subscribe(res => {
                if(res){
                    this.nameExist = res;
                    return;
                }
                this.nameExist = false;
            }, err => {});

            return this.nameExist;
        };

        this.contactClient.validateName(null, clientId, contactName).subscribe(res =>{
            if(res){
                this.nameExist = res;
                this.isFormValid = false;
                return;
            }
            this.nameExist = false;
        }, err => {});
        return  this.nameExist;
    }


    getClientsList(): Promise<PaginatedListOfClientDto> {
        return this.clientClient.get(0,0,1,null,null).toPromise();
    }

    getContactForEdit(){
        let contactId = this.activeRoute.snapshot.params['id'];
        this.contactClient.getContact(contactId).subscribe(res =>{
            this.contactEdit = true;
            this.contact = res;
            this.slectedItem.next(this.contact.clientId);
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
                if(res){
                    this.contactForm.reset();
                    this.toastrService.success("The contact has been update successfully.");
                    this.router.navigate(['/manage/clients']);
                }
            }, err => {
                this.toastrService.error("An error occurred while updating the contact.");
            });
            return;
        }
        this.contactClient.createContact(new CreateContactCommand({ clientId: this.clientId, contact: contact })).subscribe(res => {
            if(res){
                this.contactForm.reset();
                this.toastrService.success("The contact has been created successfully.");
                this.router.navigate(['/manage/clients']);
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
        this.clientId = client.id;
    }
    
    // Testing
    // Test integration select with createEntity
    // createNewClient(newClient : ClientDto){
    //     let client: ClientDto = new ClientDto({
    //         name : newClient.name,
    //         address: null,
    //         currency: null,
    //         contactList: null
    //     });
    //     this.clientClient.createClient(new CreateClientCommand({newClient: client})).subscribe(res => {
    //         switch(res){
    //             case CreateClientResult.Success:
    //                 this.toastrService.success("The contact has been created successfully.");
    //                 this.getClientsList();
    //                 break;
    //             case CreateClientResult.Error_NameExists:
    //                 this.toastrService.warning("Already exists a client with name selected.");
    //                 break;
    //             default:
    //                 this.toastrService.error("An error occurred while creating the client.");
    //                 break;       
    //         }
    //     }, err => {});
    // }
    // Testing

    onCancelClick() {
        if (this.contactEdit)
            this.router.navigate(['manage/client/', this.contact.clientId])
        else
            this.router.navigate(['manage/clients'])
    }
}
