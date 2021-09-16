import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ClientsComponent } from './clients/clients.component';
import { ClientDetailComponent } from './clients/component/client-detail/client-detail.component';
import { CreateEditClientCompontent } from './clients/component/create-edit-client/create-edit-client.component';
import { CreateEditContactComponent } from './contacts/component/create-edit-contact/create-edit-contact.component';
import { ManageComponent } from './manage.component';

const routes: Routes = [
  { path: 'manage', component: ManageComponent, children: [
    { path: 'clients', component: ClientsComponent},
    { path: 'client', component:  CreateEditClientCompontent },
    { path: 'client/contact', component:  CreateEditContactComponent },
    { path: 'client/:id', component: ClientDetailComponent}
  ]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
