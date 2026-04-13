import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { AuthPhotoComponent } from '../../../shared/Background_Photo/background';
import * as AuthDtos from '../../../shared/shared_models/Auth-models';
import { AuthService } from '../../../shared/shared_services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink, AuthPhotoComponent],
  templateUrl: './register.html',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    slug: ['', [Validators.required]],
    planId: [1],
    email: ['', [Validators.required, Validators.email, this.corporateEmailValidator]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const formData = this.form.getRawValue() as AuthDtos.TenantRegistrationDto;
    this.auth.registerTenant(formData).subscribe({
      next: (res) => {
        if (!res?.tenantId) {
          Swal.fire({
            icon: 'error',
            title: 'Registraion Error',
            text: res.message,
            confirmButtonColor: '#d33',
          });
          return;
        }

        Swal.fire({
          title: 'Registration Successful!',
          text:
            res.message || 'Your workspace is being provisioned. You will be redirected to login.',
          icon: 'success',
          confirmButtonColor: '#4F46E5',
          confirmButtonText: 'Go to Login',
          allowOutsideClick: false,
        }).then((result) => {
          if (result.isConfirmed) {
            this.router.navigate(['/login']);
          }
        });
      },
      error: (err) => {
        Swal.fire({
          icon: 'error',
          title: 'Registration Failed',
          text:
            err.error?.message ||
            'An unexpected error occurred. Please verify your inputs and try again.',
          confirmButtonText: 'Understood',
          confirmButtonColor: '#d33',
          timer: 5000,
          showClass: {
            popup: 'animate__animated animate__fadeInDown',
          },
        });
      },
    });
  }

  corporateEmailValidator(control: AbstractControl): ValidationErrors | null {
    const email = control.value as string;
    if (!email) return null;

    const publicDomains = ['gmail.com', 'yahoo.com', 'outlook.com', 'hotmail.com', 'icloud.com'];
    const domain = email.split('@')[1]?.toLowerCase();

    if (publicDomains.includes(domain)) {
      return { publicEmail: true };
    }

    return null;
  }
}
