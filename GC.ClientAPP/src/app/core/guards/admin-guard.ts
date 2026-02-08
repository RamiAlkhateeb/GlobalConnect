import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const adminGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const role = localStorage.getItem('role');

  if (role === 'Admin') {
    return true;
  }

  // Redirect unauthorized users to profile
  router.navigate(['/profile']);
  return false;
};