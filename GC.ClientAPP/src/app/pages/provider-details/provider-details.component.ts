import { CommonModule } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { AuthService } from '../../core/services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-provider-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './provider-details.component.html',
  styleUrl: './provider-details.component.scss',
})
export class ProviderDetailsComponent implements OnInit {
  route = inject(ActivatedRoute);
  authService = inject(AuthService);
  
  provider = signal<any>(null);
  loading = signal(true);

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.authService.getProviderPublic(+id).subscribe({
        next: (data) => {
          this.provider.set(data);
          this.loading.set(false);
        },
        error: () => this.loading.set(false)
      });
    }
  }
}
