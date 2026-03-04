import { HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, finalize, map, throwError } from 'rxjs';
import Swal from 'sweetalert2';
import { ApiResponse } from '../shared_models/api-response.model';
import { AuthService } from '../shared_services/auth.service';
import { LoadingService } from '../shared_services/loading.service';
import { NotificationService } from '../shared_services/notification.service';

export const appInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const loadingService = inject(LoadingService);
  const notification = inject(NotificationService);
  const router = inject(Router);

  loadingService.show();
  const token = authService.getToken();
  const currentUser = authService.currentUser();

  let authReq = req;
  if (token) {
    authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
        'X-Tenant-Id': currentUser?.tenantId || '',
      },
    });
  }

  return next(authReq).pipe(
    map((event) => {
      if (event instanceof HttpResponse) {
        const body = event.body as ApiResponse<any>;

        if (body && body.hasOwnProperty('success') && !body.success) {
          const msg = body.errors?.join(', ') || 'Operation failed';
          throw { status: 400, message: msg };
        }
      }
      return event;
    }),
    catchError((error: any) => {
      let errorMessage = 'An unknown error occurred!';

      if (error.status === 429) {
        handleRateLimitError();
      } else if (error.status === 401) {
        handleUnauthorized(authService, router);
      } else {
        errorMessage = error.message || error.error?.errors?.join(', ') || 'Server Error';
        notification.showError(errorMessage);
      }

      return throwError(() => error);
    }),
    finalize(() => loadingService.hide()),
  );
};

function handleUnauthorized(authService: any, router: any) {
  Swal.fire({
    title: 'Unauthorized Access',
    text: 'Please login With valid credentials',
    icon: 'error',
    confirmButtonText: 'try Again',
  }).then(() => {
    authService.logout();
    router.navigate(['/login']);
  });
}

function handleRateLimitError() {
  let timeLeft = 60;
  Swal.fire({
    title: 'Security Limit',
    html: `Too many attempts. Wait <b>${timeLeft}</b> seconds.`,
    icon: 'error',
    timer: 60000,
    timerProgressBar: true,
    showConfirmButton: false,
    didOpen: () => {
      const b = Swal.getHtmlContainer()?.querySelector('b');
      const interval = setInterval(() => {
        timeLeft--;
        if (b) b.textContent = timeLeft.toString();
        if (timeLeft <= 0) clearInterval(interval);
      }, 1000);
    },
  });
}
