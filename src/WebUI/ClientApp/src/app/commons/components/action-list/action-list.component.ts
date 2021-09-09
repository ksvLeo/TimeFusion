import { Component, OnInit, Injectable, Input } from '@angular/core';
import { ActionInfo } from '../../classes/action-info';

@Component({
  selector: 'app-action-list',
  templateUrl: './action-list.component.html',
  styleUrls: ['./action-list.component.css']
})
export class ActionListComponent implements OnInit {

  @Input() item: any;
  @Input() actions: ActionInfo[] = []

  constructor() {

   }

  ngOnInit(): void {
    
  }

  onClick(action) {
    action.event.emit(this.item);
  }

}
