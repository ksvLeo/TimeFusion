import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManageRoutingModule } from './manage-routing.module';
import { ClientsComponent } from './components/clients/clients.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateEditClientCompontent } from './components/clients/create-edit-client/create-edit-client.component';
import { ManageComponent } from './manage.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ContactsComponent } from './components/clients/contacts/contacts.component';
import { CreateEditContactComponent } from './components/clients/contacts/create-edit-contact/create-edit-contact.component';
import { ClientDetailComponent } from './components/clients/client-detail/client-detail.component';


@NgModule({
  declarations: [
    ManageComponent,
    ClientsComponent,
    CreateEditClientCompontent,
    ContactsComponent,
    ContactsComponent,
    CreateEditContactComponent,
    ClientDetailComponent
  ],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    ManageRoutingModule,
    AppRoutingModule,
    BrowserAnimationsModule
  ]
})
export class ManageModule { }
