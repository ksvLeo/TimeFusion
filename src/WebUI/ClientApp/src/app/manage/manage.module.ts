import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManageRoutingModule } from './manage-routing.module';
import { AppComponent } from '../app.component';
import { ClientsComponent } from './Pages/clients/clients.component';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CreateEditClientCompontent } from './components/create-edit-client/create-edit-client.component';
import { ManageComponent } from './manage.component';
import { ReactiveFormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    ManageComponent,
    ClientsComponent,
    CreateEditClientCompontent
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
