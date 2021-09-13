import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonsRoutingModule } from './commons-routing.module';
import { GridComponent } from './components/grid/grid.component';
import { ActionListComponent } from './components/action-list/action-list.component';
import { SelectComponent } from './components/select/select.component';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


@NgModule({
  declarations: [
    GridComponent,
    ActionListComponent,
    SelectComponent
  ],
  imports: [
    CommonModule,
    CommonsRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
  ],
  exports:[
    GridComponent,
    ActionListComponent,
    SelectComponent
  ]
})
export class CommonsModule { }
