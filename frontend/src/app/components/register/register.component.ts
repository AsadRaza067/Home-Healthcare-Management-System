import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { RegisterRequest } from '../../models/user.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {
  model: RegisterRequest = {
    fullName: '',
    email: '',
    password: '',
    role: 'Patient',
    phone: '',
    specialization: '',
    address: ''
  };

  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.errorMessage = '';
    this.isLoading = true;

    this.authService.register(this.model).subscribe({
      next: (res) => {
        this.isLoading = false;
        if (res.role === 'Admin') this.router.navigate(['/admin']);
        else if (res.role === 'Caregiver') this.router.navigate(['/caregiver']);
        else this.router.navigate(['/patient']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err?.error?.message || 'Registration failed. Please try again.';
      }
    });
  }
}
