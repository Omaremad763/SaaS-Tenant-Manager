import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Footer } from '../footer/footer';
import { AuthService } from '../shared_services/auth.service';
import { MENU_ITEMS } from './NavBarSections';

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './Nav-bar.html',
  imports: [RouterModule, CommonModule, Footer],
})
export class NavBarComponent {
  public authService = inject(AuthService);
  private router = inject(Router);

  isDropdownOpen = signal(false);

  filteredMenuItems = computed(() => {
    const userRoles = this.authService.currentUser()?.roles || [];

    return MENU_ITEMS.filter((item) => {
      if (!item.roles || item.roles.length === 0) return true;

      return item.roles.some((role) => userRoles.includes(role));
    });
  });

  toggleDropdown(): void {
    this.isDropdownOpen.update((v) => !v);
  }

  logout() {
    this.authService.logout();
    this.isDropdownOpen.set(false);
  }

  closeDropdown() {
    this.isDropdownOpen.set(false);
  }
}
