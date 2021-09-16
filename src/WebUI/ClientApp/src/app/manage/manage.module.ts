import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from '../app-routing.module';
import { CommonsModule } from '../commons/commons.module';
import { ClientsComponent } from './clients/clients.component';
import { ClientDetailComponent } from './clients/component/client-detail/client-detail.component';
import { CreateEditClientCompontent } from './clients/component/create-edit-client/create-edit-client.component';
import { CreateEditContactComponent } from './contacts/component/create-edit-contact/create-edit-contact.component';
import { ManageRoutingModule } from './manage-routing.module';
import { ManageComponent } from './manage.component';


@NgModule({
  declarations: [
    ManageComponent,
    ClientsComponent,
    CreateEditClientCompontent,
    ClientDetailComponent,
    CreateEditContactComponent
  ],
  imports: [
    ReactiveFormsModule,
    CommonModule,
    ManageRoutingModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CommonsModule
  ]
})
export class ManageModule { }
