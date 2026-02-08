import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';
import { NavbarComponent } from '../navbar/navbar.component';
import { User } from '../../core/models/user.model';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule, NavbarComponent],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.scss',
})
export class AdminDashboardComponent implements OnInit {
  authService = inject(AuthService);
  users = signal<User[]>([]);
  isUpdating: number | null = null;

  ngOnInit() {
    console.log("Length", this.users.length);
    this.loadProviders();
  }

  loadProviders() {
    this.authService.getProviders().subscribe({
      next: (data) =>  {
        this.users.set(data);
        console.log("data", this.users);
        console.log("Length", this.users().length);
      },
      error: (err) => console.error(err)
    });
  }

  toggleStatus(user: any) {
    const idToUpdate = user.providerId || user.id; 

    this.isUpdating = idToUpdate;
    this.authService.toggleProviderStatus(user.id).subscribe({
      next: () => {
        // CHANGE 3: Update a single item inside the signal
        this.users.update(currentList => 
          currentList.map(u => {
            // Check both potential ID names to be safe
            const uId = (u as any).providerId || u.id;
            if (uId === idToUpdate) {
              return { ...u, isActive: !u.isActive };
            }
            return u;
          })
        );
        this.isUpdating = null;
      },
      error: () => {
        alert("Failed to update status");
        this.isUpdating = null;
      }
    });
  }
}
