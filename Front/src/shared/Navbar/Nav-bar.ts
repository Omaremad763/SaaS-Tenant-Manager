import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../shared_services/auth.service';
import { MENU_ITEMS } from './NavBarSections';

@Component({
  selector: 'app-Navbar',
  standalone: true,
  templateUrl: './Nav-bar.html',
  styleUrls: ['./Nav-bar.css'],
  imports: [RouterModule, CommonModule],
})
export class NavBarComponent {
  menuItems = MENU_ITEMS;
  router = inject(Router);
  authService = inject(AuthService);
  public isDropdownOpen: boolean = false;
  public toggleDropdown(): void {
    this.isDropdownOpen = !this.isDropdownOpen;
  }
  logout() {
    this.authService.logout();
  }
}
