// main-layout.component.ts
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '../shared_services/auth.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  template: `
    <nav class="bg-gray-800 p-4 text-white flex justify-between items-center shadow-lg">
      <div class="font-black text-xl tracking-tight">SaaS APP</div>
      <button
        (click)="logout()"
        class="px-4 py-2 bg-red-600 rounded-lg font-bold hover:bg-red-700 transition-all"
      >
        Logout
      </button>
    </nav>

    <main class="min-h-screen bg-slate-50">
      <router-outlet></router-outlet>
    </main>
  `,
})
export class MainLayoutComponent {
  private authService = inject(AuthService);
  logout() {
    this.authService.logout();
  }
}
