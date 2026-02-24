import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { LoadingService } from '../shared_services/loading.service';

@Component({
  selector: 'app-loading-spinner',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './loading-spinner.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoadingSpinner {
  constructor(public loadingService: LoadingService) {}
}