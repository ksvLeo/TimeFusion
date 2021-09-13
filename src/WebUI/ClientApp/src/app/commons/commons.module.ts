import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonsRoutingModule } from './commons-routing.module';
import { GridComponent } from './components/grid/grid.component';
import { ActionListComponent } from './components/action-list/action-list.component';
import { GenericModalComponent } from './components/generic-modal/generic-modal.component';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';


@NgModule({
  declarations: [
    GridComponent,
    ActionListComponent,
    GenericModalComponent
  ],
  imports: [
    CommonModule,
    CommonsRoutingModule,
    NgbModule
  ],
  exports:[
    GridComponent,
    ActionListComponent
  ],
  providers: [NgbActiveModal]
})
export class CommonsModule { }
