import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageRoutingModule } from './manage-routing.module';
import { ClientsComponent } from './components/clients/clients.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateEditClientCompontent } from './components/clients/create-edit-client/create-edit-client.component';
import { ManageComponent } from './manage.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateEditContactComponent } from './components/clients/create-edit-contact/create-edit-contact.component';
import { CommonsModule } from '../commons/commons.module';
import { ClientDetailComponent } from './components/clients/client-detail/client-detail.component';


@NgModule({
  declarations: [
    ManageComponent,
    ClientsComponent,
    CreateEditClientCompontent,
    CreateEditContactComponent,
    ClientDetailComponent
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
