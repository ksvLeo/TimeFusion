import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { Subject } from "rxjs";
import { ClientClient, ClientDto, ContactClient, ContactDto, CreateClientCommand, CreateContactCommand, UpdateContactCommand } from "src/app/web-api-client";



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

    // Testing
    test = new Subject<ClientDto[]>();

    // Testing

    
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

        this.contactForm = this.fb.group({
            client: [ "", [Validators.required]],
            title: ["", [Validators.minLength(3)]],
            name: [ "", [Validators.minLength(3), Validators.required]],
            email: [ "", [Validators.email]],
            phone: ["", []],
        });

        this.contactForm.valueChanges.subscribe(changes => {
            this.contactFormChanges(changes);
        });

        var url = this.router.url.split("/");

        switch (url[4]) {
            case "create":
                if (url.length == 4) {
                    this.getClients();    
                } else {
                    this.getClientById(Number(url[5])).then((res: ClientDto) => {
                        this.clients.push(res);
                        this.contactForm.get('client').patchValue(res.id)
                    })
                }
                break;
            case "edit":
                this.getContactForEdit();
                break;
            default:

                break;
        }
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

    getClientById(id: number): Promise<ClientDto> {
        return this.clientClient.getClient(id).toPromise()
    }

    getContactForEdit(){
        this.activeRoute.params.subscribe(res => {
            if(res.id == null){
                return;
            }
            this.contactClient.getContact(res.id).subscribe(res =>{
                this.contactEdit = true;
                this.contact = res;
                this.contactForm.setValue({
                    client: [ this.contact.clientId],
                    title: [ this.contact.title],
                    name: [ this.contact.name],
                    email: [ this.contact.email],
                    phone: [ this.contact.phoneNumber]
                });
                this.contactForm.controls.title.setErrors(null);
                this.contactForm.controls.email.setErrors(null);
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

    // Testing
    processClientId(id: number){
        console.log(id);
    }
    
    getClientsForName(name: string){
        this.clientClient.getClientsByName(name).subscribe(res => {
            this.clients = res;
            this.test.next(this.clients);
        }, err => {});    
    }

    createNewClient(newClient: ClientDto){
        this.clientClient.createClient(new CreateClientCommand({newClient: newClient})).subscribe(res => {
            this.getClientsForName(newClient.name);
        });
    }
    // Testing
}
