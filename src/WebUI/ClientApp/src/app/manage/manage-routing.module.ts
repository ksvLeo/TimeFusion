import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateEditClientCompontent } from './components/create-edit-client/create-edit-client.component';
import { ManageComponent } from './manage.component';
import { ClientsComponent } from './Pages/clients/clients.component';

const routes: Routes = [
  { path: 'manage', component: ManageComponent, children: [
    { path: 'clients', component: ClientsComponent},
    { path: 'clients/create', component:  CreateEditClientCompontent },
    { path: 'clients/edit/:id', component:  CreateEditClientCompontent }
  ]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
