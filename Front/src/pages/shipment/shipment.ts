import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { ShipmentsService } from '../../core/Services/ShipmentsService';
import { ShipmentDto, ShipmentStatus, UpdateShipmentDTO } from '../../core/core-models';

@Component({
  selector: 'app-shipment-management',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './shipment.html',
})
export class ShipmentManagementComponent implements OnInit {
  shipments: ShipmentDto[] = [];
  filteredShipments: ShipmentDto[] = [];
  shipmentStatuses = Object.values(ShipmentStatus);
  isLoading = true;
  searchTerm = '';
  statusFilter = '';

  showModal = false;
  shipmentModel: Partial<ShipmentDto> = {};

  constructor(private operationsService: ShipmentsService) {}

  ngOnInit(): void {
    this.loadShipments();
  }

  loadShipments(): void {
    this.isLoading = true;
    this.operationsService.getTenantShipments().subscribe({
      next: (res) => {
        if (res.success) {
          this.shipments = res.data;
          this.filteredShipments = res.data;
        }
        this.isLoading = false;
      },
      error: () => (this.isLoading = false),
    });
  }

  applyFilter(): void {
    this.filteredShipments = this.shipments.filter((s) => {
      const matchesSearch =
        s.trackingNumber.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        s.receiverName.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesStatus = this.statusFilter ? s.status === this.statusFilter : true;
      return matchesSearch && matchesStatus;
    });
  }

  getStatusClass(status: string): string {
    const base = 'px-3 py-1 rounded-full text-xs font-medium ';
    switch (status) {
      case 'Delivered':
        return base + 'bg-green-100 text-green-700';
      case 'Pending':
        return base + 'bg-yellow-100 text-yellow-700';
      case 'Cancelled':
        return base + 'bg-red-100 text-red-700';
      default:
        return base + 'bg-gray-100 text-gray-700';
    }
  }

  openModal(shipment?: ShipmentDto): void {
    this.shipmentModel = shipment ? { ...shipment } : {};
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
    this.shipmentModel = {};
  }

  UpdateShipmentStatus(): void {
    if (!this.shipmentModel.id) {
      Swal.fire({
        icon: 'error',
        title: 'Invalid Operation',
        text: 'Cannot update shipment without an ID.',
      });
      return;
    }

    const DTO: UpdateShipmentDTO = {
      id: this.shipmentModel.id!,
      status: this.shipmentModel.status as ShipmentStatus,
    };

    this.operationsService.updateShipmentStatus(DTO).subscribe({
      next: (res) => {
        if (res.success) {
          Swal.fire({
            icon: 'success',
            title: 'Shipment Updated',
            text: 'The shipment status has been updated successfully.',
          });
          this.loadShipments();
          this.closeModal();
        } else {
          Swal.fire({
            icon: 'error',
            title: 'Update Failed',
            text: 'Something went wrong while updating the shipment.',
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
}
