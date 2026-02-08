import { Component, inject, OnInit, signal } from '@angular/core';
import { NavbarComponent } from '../navbar/navbar.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  authService = inject(AuthService);

  // Form Data Model
  profile = {
    name: '',
    email: '',
    mobileNumber: '',
    specialty: '',
    bio: '',
    googleBookingUrl: ''
  };

  isLoading = signal(true); 
  isSaving = signal(false);
  message = signal({ text: '', type: '' });

  ngOnInit() {
    this.loadProfile();
  }

  loadProfile() {
    // We assume your AuthService has a method to get the *current* user's profile
    // If not, you can reuse the /api/profile endpoint we built
    this.authService.getProfile().subscribe({
      next: (data: any) => {
        this.profile = {
          name: data.name,
          email: data.email,
          mobileNumber: data.mobileNumber, // Read-only usually
          specialty: data.specialty || '',
          bio: data.bio || '',
          googleBookingUrl: data.googleBookingUrl || ''
        };
        console.log('Profile data loaded:', data);
        // TRIGGER UI UPDATE
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.isLoading.set(false);
      }
    });
  }

  onSubmit() {
    this.isSaving.set(true);
    this.message.set({ text: '', type: '' });

    this.authService.updateProfile(this.profile).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.message.set({ text: 'Profile updated successfully!', type: 'success' });

        // Update the global signal so Navbar shows new name immediately
        this.authService.currentUser.update(user => 
          user ? { ...user, name: this.profile.name } : null
        );
      },
      error: (err) => {
        this.isSaving.set(false);
        this.message.set({ text: 'Failed to update profile.', type: 'error' });
        console.error(err);
      }
    });
  }
}