import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import Swal from 'sweetalert2';
import { SubscriptionPlanDetailsDto, TenantManagement } from '../../core/core-models';
import { SubscriptioDashboard_service } from '../../core/SubscriptioDashboard_service';
import { AuthService } from '../../shared/shared_services/auth.service';

@Component({
  selector: 'app-subscription-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
})
export class SubscriptionDashboardComponent implements OnInit {
  private subService = inject(SubscriptioDashboard_service);
  private authService = inject(AuthService);

  user = this.authService.currentUser;

  tenantId = computed(() => this.user()?.tenantId || '');
  roles = computed(() => this.user()?.roles || []);

  isSystemAdmin = computed(() => this.roles().includes('SystemAdmin'));
  isTenantAdmin = computed(() => this.roles().includes('TenantAdmin'));
  isTenantUser = computed(() => this.roles().includes('TenantUser'));

  TenantData = signal<TenantManagement[]>([]);
  subscriptionData = signal<SubscriptionPlanDetailsDto | null>(null);
  ngOnInit() {
    this.checkAccessAndLoad();
  }

  checkAccessAndLoad() {
    if (this.isTenantUser()) {
      Swal.fire({
        title: 'Access Denied',
        text: 'You do not have permission to access this page.',
        icon: 'error',
        confirmButtonColor: '#3085d6',
      });
      return;
    }

    if (this.isSystemAdmin()) {
      this.LoadAllTenants();
    } else if (this.isTenantAdmin() && this.tenantId()) {
      this.loadTenantSubscriptionByID(this.tenantId());
    }
  }

  loadTenantSubscriptionByID(id: string) {
    this.subService.getTenantSubscriptionByTenantId(id).subscribe((res) => {
      if (res.success) this.subscriptionData.set(res.data);
    });
  }

  LoadAllTenants() {
    this.subService.GetTenantSubscriptionData().subscribe((res) => {
      if (res.success) this.TenantData.set(res.data);
    });
  }

  onToggleFeature(featureName: string, event: any) {
    if (!this.isSystemAdmin()) {
      Swal.fire('Unauthorized', 'Only System Admins can toggle features.', 'warning');
      event.target.checked = !event.target.checked;
      return;
    }

    const isEnabled = event.target.checked;
    this.subService
      .toggleFeature({
        tenantId: this.tenantId(),
        FeatureName: featureName,
        isEnabled: isEnabled,
      })
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.showToast(`Feature ${featureName} updated successfully!`, 'success');
          }
        },
        error: () => {
          event.target.checked = !isEnabled;
          Swal.fire('Error', 'Update failed.', 'error');
        },
      });
  }

  private showToast(message: string, icon: any) {
    Swal.fire({
      title: message,
      icon: icon,
      toast: true,
      position: 'top-end',
      timer: 2000,
      showConfirmButton: false,
    });
  }
}
