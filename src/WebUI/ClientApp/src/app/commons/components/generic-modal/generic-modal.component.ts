import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-generic-modal',
  templateUrl: './generic-modal.component.html',
  styleUrls: ['./generic-modal.component.css']
})
export class GenericModalComponent implements OnInit {

  title: string;
  message: string;

  constructor(public activeModal: NgbActiveModal) {

  }
      
  ngOnInit(): void {

  }
     
  closeModal(message: string) {
    this.activeModal.close();
  }
  
}