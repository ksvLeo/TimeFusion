import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { SelectInfo } from "src/app/shared/interfaces/selectInfo";
import { ClientClient, ClientDto, ContactClient, ContactDto, CreateClientCommand, CreateContactCommand, UpdateContactCommand } from "src/app/web-api-client";
import { ToastrService } from 'ngx-toastr';
import { Subject } from "rxjs";

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
    selectInfo : SelectInfo = {
        buttonCreateName: "Create Client",
        isButtonCreated: false,
        label: 'Client',
        required: true
    }
    // Testing

    //Edit
    contactEdit: boolean = false;
    contact: ContactDto;
    editItemId: number;
    slectedItem: Subject<number> = new Subject<number>();

    constructor(
        private fb: FormBuilder,
        private clientClient: ClientClient,
        private contactClient: ContactClient,
        private activeRoute: ActivatedRoute,
        private router: Router,
        private toastrService: ToastrService 
    ){}

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

        // Testing
        this.activeRoute.params.subscribe(res => {
            console.log(res);
            if(res.mode == "create"){
                this.getClients();

                if(this.clients != null && res.id != null){
                    let r =this.clients.find(c => c.id == res.id)
                    this.slectedItem.next(r.id);

                    return;
                }
            }

            if(res.mode == "edit"){
                this.getClients();
                this.getContactForEdit();
                return;
            }
            
            if(res.mode != "create" && res.mode != "edit"){
                this.router.navigate(['/manage/clients/']);

            }
        });
        // Testing

        // var urlParams = this.activeRoute.snapshot.params
        // console.log(urlParams['mode'])
        // switch (urlParams['mode']) {
        //     case "create":
        //         if (urlParams['id'] == null) {
        //             this.getClients();    
        //         } else {
        //             this.getClientById(Number(urlParams['id'])).then((res: ClientDto) => {
        //                 this.clients.push(res);
        //                 this.contactForm.get('client').patchValue(res.id);
        //             })
        //         }
        //         break;
        //     case "edit":
        //         this.getClients();
        //         this.getContactForEdit();
        //         break;
        //     default:
        //         this.router.navigate['/management/clients/']
        //         break;
        // }
    }

    contactFormChanges($values){
        if(!this.contactEdit && !$values || this.editItemId == null || $values.name.length < 3){
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
        this.isFormValid = this.contactForm.valid && !this.contactNameExists(this.editItemId,+$values.client, $values.name);
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
        }, err => {
            this.toastrService.error("An error occurred while obtaining the clients.");
        });
    }

    getClientById(id: number): Promise<ClientDto> {
        return this.clientClient.getClient(id).toPromise();
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
            this.contactClient.updateContact(new UpdateContactCommand({ clientId: +this.contactForm.get('client').value, newContact: contact})).subscribe(res => {
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
        this.contactClient.createContact(new CreateContactCommand({ clientId: +this.contactForm.get('client').value, contact: contact })).subscribe(res => {
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

    // Testing
    processClientId(id: number){
    }

    createNewClient(newClient : ClientDto){
        let client: ClientDto = new ClientDto({
            name : newClient.name,
            address: null,
            currency: null,
            contactList: null
        });
        this.clientClient.createClient(new CreateClientCommand({newClient: client})).subscribe(res => {
            this.toastrService.success("The contact has been update successfully.");
            this.getClients();
        }, err => {
            this.toastrService.error("An error occurred while creating the client.");

        });
    }

    onCancelClick() {
        if (this.contactEdit)
            this.router.navigate(['manage/clients/', this.contact.clientId])
        else
            this.router.navigate(['manage/clients'])
    }
    // Testing
}
