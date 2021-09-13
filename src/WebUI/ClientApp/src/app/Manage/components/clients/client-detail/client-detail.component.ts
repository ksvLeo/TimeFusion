import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientClient, ClientDto, ContactDto, CurrencyDto } from 'src/app/web-api-client';

@Component({
  selector: 'app-client-detail',
  templateUrl: './client-detail.component.html',
  styleUrls: ['./client-detail.component.css']
})
export class ClientDetailComponent implements OnInit {

  clientInfo: ClientDto = new ClientDto({ currency: new CurrencyDto() })

  constructor(private clientService: ClientClient,
              private router: Router,
              private route: ActivatedRoute) { }

  ngOnInit(): void {

    //Falta cargar proyectos y contactos
    //Por ahora que es poco podría entrar todo en ngOnInit, habrá que ver a medida que lo vayamos definiendo esto
    this.clientService.getClient(Number(this.route.snapshot.paramMap.get('id'))).subscribe((res: ClientDto) => {
      this.clientInfo = res
    })

  }

}
