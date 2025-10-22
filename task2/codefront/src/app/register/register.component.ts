import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  dateOfBirth = '';
  designation: 'Teacher' | 'Student' = 'Student';
  errorMsg = '';
  successMsg = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.name || !this.email || !this.password || !this.dateOfBirth) {
      this.errorMsg = 'Please fill all fields';
      return;
    }

    this.loading = true;
    this.errorMsg = '';
    this.successMsg = '';

    const user = {
      name: this.name,
      email: this.email,
      password: this.password,
      dateOfBirth: new Date(this.dateOfBirth),
      designation: this.designation
    };

    this.authService.register(user).subscribe({
      next: () => {
        this.loading = false;
        this.successMsg = 'Registration successful! Redirecting to login...';
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (error) => {
        this.loading = false;
        this.errorMsg = 'Registration failed. Please try again.';
        console.error('Registration error:', error);
      }
    });
  }
}
