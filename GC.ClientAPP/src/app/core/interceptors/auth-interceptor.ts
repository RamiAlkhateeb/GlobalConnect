import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  
  // Get the token (ensure your AuthService has a getToken() method)
  // If you don't have a getter, you can use: localStorage.getItem('token')
  const token = authService.getToken(); 

  if (token) {
    // Clone the request to add the new header
    const clonedReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
    return next(clonedReq);
  }

  // If no token, send original request
  return next(req);
};
