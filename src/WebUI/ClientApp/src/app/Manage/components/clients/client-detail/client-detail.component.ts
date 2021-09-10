import { Component, OnInit } from '@angular/core';
import { ClientClient, ClientDto, ContactDto, CurrencyDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent implements OnInit {

  clientInfo: ClientDto = new ClientDto({ currency: new CurrencyDto() })

  constructor(private clientService: ClientClient) { }

  ngOnInit(): void {
    this.onLoad()
    console.log(this.clientInfo)
  }

  onLoad() { //Load Client Info + Projects
      this.clientService.getClient(9).subscribe((response) => {
          this.clientInfo = response
      })
  }
}
