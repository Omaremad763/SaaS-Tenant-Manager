import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../shared_services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const currentUser = authService.currentUser();

  if (authService.isAuthenticated() && currentUser) {
    const expectedRole = route.data['role'] as string;

    if (expectedRole && !currentUser.roles.includes(expectedRole)) {
      router.navigate(['/unauthorized']);
      return false;
    }

    return true;
  }

  router.navigate(['/login'], {
    queryParams: { returnUrl: state.url },
    replaceUrl: true,
  });

  return false;
};
