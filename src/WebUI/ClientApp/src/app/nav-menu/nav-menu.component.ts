import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {

  navItems: NavMenuItemInterface[] = [
    { 
      description: "",
      url: "/",
      icon:"fas fa-laptop-house fa-2x"
    },
    {
      description: "Time",
      url: "/"
    },
    {
      description: "Expenses",
      url: "/"
    },
    {
      description: "Projects",
      url: "/"
    },
    {
      description: "Team",
      url: "/"
    },
    {
      description: "Reports",
      url: "/"
    },
    {
      description: "Invoices",
      url: "/"
    },
    {
      description: "Manage",
      url: "/manage"
    },
    {
      description: "API",
      url: "/api"
    },
    {
      description: "",
      url: "/",
      icon: 'far fa-user fa-2x'
    }
  ]

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}

export interface NavMenuItemInterface{
    url: string;
    description: string;
    icon?: string;
    
}
