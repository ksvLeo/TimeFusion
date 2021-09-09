import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonsRoutingModule } from './commons-routing.module';
import { GridComponent } from './components/grid/grid.component';
import { ActionListComponent } from './components/action-list/action-list.component';


@NgModule({
  declarations: [
    GridComponent,
    ActionListComponent
  ],
  imports: [
    CommonModule,
    CommonsRoutingModule
  ],
  exports:[
    GridComponent,
    ActionListComponent
  ]
})
export class CommonsModule { }
