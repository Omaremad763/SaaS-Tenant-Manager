import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { AuthPhotoComponent } from '../../../shared/Background_Photo/background';
import * as AuthDtos from '../../../shared/shared_models/Auth-models';
import { AuthService } from '../../../shared/shared_services/auth.service';

@Component({
  selector: 'app-register-user',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, AuthPhotoComponent],
  templateUrl: './register-user.html',
})
export class RegisterUserComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  form = this.fb.group({
    email: ['', { validators: [Validators.required, Validators.email], nonNullable: true }],
    password: ['', { validators: [Validators.required], nonNullable: true }],
  });
  onSubmit() {
    if (this.form.invalid) return;
    const formData = this.form.getRawValue() as AuthDtos.TenantRegistrationDto;
    this.auth.registerUser(formData).subscribe({
      next: (response: any) => {
        console.warn('Registration response:', response);
        const isActualSuccess =
          response.toLowerCase().includes('successfully') ||
          response.toLowerCase().includes('linked');
        if (isActualSuccess) {
          Swal.fire({
            title: 'Welcome On Board!',
            text: 'Succesful Registration, Going to Login Page',
            icon: 'success',
            timer: 3000,
            timerProgressBar: true,
            showConfirmButton: false,
            background: '#ffffff',
            iconColor: '#rgb(14, 228, 3)',
          }).then(() => {
            this.router.navigateByUrl('/login', { replaceUrl: true });
          });
        } else {
          Swal.fire({
            title: 'Registration Note',
            text: response,
            icon: 'warning',
            confirmButtonText: 'Try Again',
            confirmButtonColor: '#2563eb',
          });
        }
      },
      error: (err) => {
        Swal.fire({
          title: 'System Error',
          text: 'Something went wrong on our servers. Please try again later.',
          icon: 'error',
          confirmButtonColor: '#ef4444',
        });
      },
    });
  }
}
