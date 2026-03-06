import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { ClientService } from '../../core/Services/client-service';

@Component({
  selector: 'app-client-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './client.html',
})
export class ClientManagementComponent implements OnInit {
  clients: any[] = [];
  isLoading = false;
  showModal = false;
  clientModel: any = { name: '', phone: '', address: '' };

  constructor(private clientService: ClientService) {}

  ngOnInit(): void {
    this.loadClients();
  }

  loadClients(): void {
    this.isLoading = true;
    this.clientService.getAllClients().subscribe({
      next: (res) => {
        if (res.success) this.clients = res.data;
        this.isLoading = false;
      },
      error: () => (this.isLoading = false),
    });
  }

  saveClient(): void {
    const isEdit = !!this.clientModel.id;

    const action = isEdit
      ? this.clientService.updateClient(this.clientModel)
      : this.clientService.createClient(this.clientModel);

    action.subscribe({
      next: (res) => {
        if (res.success) {
          Swal.fire({
            icon: 'success',
            title: isEdit ? 'Client Updated' : 'Client Created',
            text: isEdit
              ? 'The client has been updated successfully.'
              : 'The client has been created successfully.',
            confirmButtonText: 'OK',
          });

          this.loadClients();
          this.closeModal();
        } else {
          Swal.fire({
            icon: 'error',
            title: 'Operation Failed',
            text: 'Something went wrong while saving the client.',
          });
        }
      },
      error: () => {
        Swal.fire({
          icon: 'error',
          title: 'Server Error',
          text: 'Unable to complete the request.',
        });
      },
    });
  }

  deleteClient(id: string): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you really want to delete this client?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#ef4444',
      cancelButtonColor: '#6b7280',
      confirmButtonText: 'Yes, delete it',
      cancelButtonText: 'Cancel',
    }).then((result) => {
      if (result.isConfirmed) {
        this.clientService.deleteClient(id).subscribe((res) => {
          if (res.success) {
            Swal.fire({
              icon: 'success',
              title: 'Deleted',
              text: 'Client has been deleted successfully',
            });
            this.loadClients();
          } else {
            Swal.fire({
              icon: 'error',
              title: 'Error',
              text: 'Failed to delete the client',
            });
          }
        });
      }
    });
  }

  openModal(client?: any): void {
    this.clientModel = client ? { ...client } : { name: '', email: '', phone: '', address: '' };
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }
}
