import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

// Interfaces for Type Safety
export interface LoginResponse {
  token: string;
  role: string;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = 'https://localhost:7143/api'; // Ensure this matches your .NET port

  // Signal to track current user state across the app
  currentUser = signal<{ name: string; role: string } | null>(null);

  constructor() {
    // Restore session on page refresh
    const token = localStorage.getItem('token');
    const role = localStorage.getItem('role');
    const name = localStorage.getItem('name');
    if (token && role && name) {
      this.currentUser.set({ name, role });
    }
  }

  register(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/register`, data);
  }

  login(data: any): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, data).pipe(
      tap((res) => {
        // 1. Save to Local Storage
        localStorage.setItem('token', res.token);
        localStorage.setItem('role', res.role);
        localStorage.setItem('name', res.name);

        // 2. Update Signal
        this.currentUser.set({ name: res.name, role: res.role });
      })
    );
  }

  logout() {
    localStorage.clear();
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  // --- Admin Methods ---
  getProviders(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/admin/providers`, {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    });
  }

  toggleProviderStatus(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/admin/toggle-status/${id}`, {}, {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    });
  }

  // --- Profile Methods ---
  updateProfile(data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/providers`, data, {
      headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }
    });
  }

  getProfile() {
    return this.http.get(`${this.apiUrl}/providers`);
  }

  // Add this method inside your AuthService class
  getToken(): string | null {
    // Check for platform browser if using SSR, otherwise just:
    return localStorage.getItem('token');
  }

  getProviderPublic(id: number) {
    return this.http.get<any>(`${this.apiUrl}/public/provider/${id}`);
  }
}