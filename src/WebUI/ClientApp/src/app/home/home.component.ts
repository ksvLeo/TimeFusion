import { Component } from '@angular/core';
import { CurrencyReferenceClient } from '../web-api-client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  constructor(private currencyReferenceClient: CurrencyReferenceClient){
    this.currencyReferenceClient.getCurrencyReferences().subscribe(res => {
      console.log(res);
    });
  }
}
