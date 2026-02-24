import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-photo',
  standalone: true,
  template: `
    <div class="relative min-h-screen flex items-center justify-center overflow-hidden">
      <!-- Background -->
      <img
        [src]="imgSrc"
        class="absolute inset-0 w-full h-full object-cover"
        alt="Auth background"
      />

      <!-- Overlay -->
      <div class="absolute inset-0"></div>

      <!-- Content -->
      <div class="relative z-10 w-full flex justify-center">
        <ng-content></ng-content>
      </div>
    </div>
  `,
})
export class AuthPhotoComponent {
  @Input() imgSrc = '';
}
