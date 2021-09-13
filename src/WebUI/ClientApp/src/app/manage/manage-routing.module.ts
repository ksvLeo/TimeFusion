import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateEditClientCompontent } from './components/clients/create-edit-client/create-edit-client.component';
import { ManageComponent } from './manage.component';
import { ClientsComponent } from './components/clients/clients.component';
import { CreateEditContactComponent } from './components/clients/create-edit-contact/create-edit-contact.component';

const routes: Routes = [
  { path: 'manage', component: ManageComponent, children: [
    { path: 'clients', component: ClientsComponent},
    { path: 'clients/create', component:  CreateEditClientCompontent },
    { path: 'clients/edit/:id', component:  CreateEditClientCompontent },
    { path: 'clients/contact/create', component:  CreateEditContactComponent },
    { path: 'clients/contact/edit/:id', component:  CreateEditContactComponent }
  ]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
