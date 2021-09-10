import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ClientClient, ClientDto, ContactClient, ContactDto, CreateContactCommand, UpdateContactCommand } from "src/app/web-api-client";



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
    
    //Edit
    contactEdit: boolean = false;
    contact: ContactDto;
    contactId: number = null;


    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private contactClient: ContactClient,
        private activeRoute: ActivatedRoute,
        private router: Router
    ){}

    ngOnInit(){
        this.getContactForEdit();
        this.getClients();
        
        this.contactForm = this.fb.group({
            client: [ "", [Validators.required]],
            title: ["", []],
            name: [ "", [ Validators.required]],
            email: [ "",],
            phone: [ "", []]
        });
        
        this.contactForm.valueChanges.subscribe(changes => {
            this.contactFormChanges(changes);
        });
    }

    contactFormChanges($values){
        if(!this.contactEdit && !$values || isNaN(parseInt($values.client)) || $values.name.length < 3){
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

        this.isFormValid = this.contactForm.valid && !this.contactNameExists(this.contactId,+$values.client, $values.name);
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

    getClients(){
        this.clientClient.get(0,0,1, null).subscribe(res => {
            this.clients = res.items;
        }, err => {});
    }

    getContactForEdit(){
        this.activeRoute.params.subscribe(res => {
            if(res.id == null){
                return;
            }
            this.contactClient.getContact(res.id).subscribe(res =>{
                this.contactEdit = true;
                this.contact = res;
                this.contactForm.patchValue({
                    client: [ this.contact.clientId ],
                    title: [ this.contact.title],
                    name: [ this.contact.name],
                    email: [ this.contact.email],
                    phone: [ this.contact.phoneNumber]
                });

            });
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
            this.contactClient.updateContact(new UpdateContactCommand({ clientId: +this.contactForm.get('client').value, newContact: contact})).subscribe(res => {
                if(res){
                    this.contactForm.reset();
                    this.router.navigate(['/manage/clients']);
                }
            }, err => {});
            return;
        }
        this.contactClient.createContact(new CreateContactCommand({ clientId: +this.contactForm.get('client').value, contact: contact })).subscribe(res => {
            if(res){
                this.contactForm.reset();
                this.router.navigate(['/manage/clients']);
            }
        }, err => {});
    }

    mapContact(contactForm: FormGroup): ContactDto{
        return new ContactDto({
            name: contactForm.get('name').value,
            email: contactForm.get('email').value,
            title: contactForm.get('title').value,
            phoneNumber: contactForm.get('phone').value
        });
    }
}