import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, ValidationErrors, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { ClientClient, ClientDto, ContactClient, ContactDto } from "src/app/web-api-client";



@Component({
    selector: 'app-create-edit-contact-component',
    templateUrl: './create-edit-contact.component.html',
    styleUrls: ['./create-edit-contact.component.scss']
})
export class CreateEditContactComponent implements OnInit {

    // Form
    contactForm: FormGroup;
    isFormsValid: boolean = true;

    // Data
    clients: ClientDto[] = [];
    
    //Edit
    contactEdit: boolean = false;
    contact: ContactDto;


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
            title: ["", [Validators.required]],
            name: [ "", [Validators.minLength(3), Validators.required]],
            email: [ "", [Validators.email]],
            phone: [ "", []]
        });

        this.contactForm.valueChanges.subscribe(changes => {
            this.contactFormChanges(changes);
        });
        
    }

    contactFormChanges($values){
        this.isFormsValid = this.contactForm.valid;
    }

    getClients(){
        // this.clientClient.getClientsByName(this.contactForm.get('name').value).subscribe(res => {
        //     this.clients = res;
        // }, err =>  {});
    }

    saveContact(){
    }
}