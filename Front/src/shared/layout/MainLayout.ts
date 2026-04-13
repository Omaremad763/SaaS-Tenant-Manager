// main-layout.component.ts
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { NavBarComponent } from '../Navbar/Nav-bar';
import { AuthService } from '../shared_services/auth.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, NavBarComponent],
  template: ` <app-navbar></app-navbar> `,
})
export class MainLayoutComponent {
  private authService = inject(AuthService);
  logout() {
    this.authService.logout();
  }
}
