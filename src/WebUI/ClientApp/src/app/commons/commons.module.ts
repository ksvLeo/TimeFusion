import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CommonsRoutingModule } from './commons-routing.module';
import { GridComponent } from './components/grid/grid.component';


@NgModule({
  declarations: [
    GridComponent
  ],
  imports: [
    CommonModule,
    CommonsRoutingModule
  ],
  exports:[
    
  ]
})
export class CommonsModule { }
