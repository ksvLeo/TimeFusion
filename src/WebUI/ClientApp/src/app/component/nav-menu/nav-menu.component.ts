import { Component } from '@angular/core';
import { NavMenuItemInterface } from '../../interfaces/navMenuItem';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {


  navItems: NavMenuItemInterface[] = [
    {
      description: "Time",
      url: "**"
    },
    {
      description: "Expenses",
      url: "**"
    },
    {
      description: "Projects",
      url: "**"
    },
    {
      description: "Team",
      url: "**"
    },
    {
      description: "Reports",
      url: "**"
    },
    {
      description: "Invoices",
      url: "**"
    },
    {
      description: "Manage",
      url: "/manage"
    },
  ]

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
