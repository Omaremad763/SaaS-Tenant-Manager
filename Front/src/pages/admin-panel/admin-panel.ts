import { animate, query, stagger, style, transition, trigger } from '@angular/animations';
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import Swal from 'sweetalert2';
import { SystemMetrics } from '../../core/core-models';
import { AdminPanel_service } from '../../core/Services/AdminPanel_service';

@Component({
  selector: 'app-admin-panel',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-panel.html',
  animations: [
    trigger('listAnimation', [
      transition('* <=> *', [
        query(
          ':enter',
          [
            style({ opacity: 0, transform: 'translateY(20px)' }),
            stagger('100ms', [
              animate('500ms ease-out', style({ opacity: 1, transform: 'translateY(0)' })),
            ]),
          ],
          { optional: true },
        ),
      ]),
    ]),
    trigger('fadeSlide', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateX(-10px)' }),
        animate('400ms ease-out', style({ opacity: 1, transform: 'translateX(0)' })),
      ]),
    ]),
  ],
})
export class AdminPanel implements OnInit {
  private metricsService = inject(AdminPanel_service);

  metrics = signal<SystemMetrics | null>(null);
  isLoading = signal(true);

  ngOnInit() {
    this.loadMetrics();
  }

  loadMetrics() {
    this.isLoading.set(true);
    // تأخير بسيط لإظهار جمال الـ Skeleton Loader (اختياري)
    setTimeout(() => {
      this.metricsService.GetSystemMetrics().subscribe({
        next: (res) => {
          if (res.success) {
            this.metrics.set(res.data);
            this.isLoading.set(false);
            this.showSuccessToast();
          }
        },
        error: (err) => {
          this.isLoading.set(false);
          this.showErrorAlert();
        },
      });
    }, 600);
  }

  private showSuccessToast() {
    Swal.fire({
      toast: true,
      position: 'top-end',
      icon: 'success',
      title: 'Real-time data synced',
      showConfirmButton: false,
      timer: 2000,
      background: '#f8fafc',
      iconColor: '#3b82f6',
    });
  }

  private showErrorAlert() {
    Swal.fire({
      title: 'Sync Failed',
      text: 'Check your connection to the monitoring server.',
      icon: 'error',
      confirmButtonColor: '#3b82f6',
    });
  }
}
