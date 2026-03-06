import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { ClientService } from '../../core/Services/client-service';
import { ShipmentsService } from '../../core/Services/ShipmentsService';

@Component({
  selector: 'app-create-shipment-wizard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-shipment-wizard.html',
})
export class CreateShipmentWizardComponent implements OnInit {
  currentStep = 1;
  shipmentForm: FormGroup;
  clients: any[] = [];

  constructor(
    private fb: FormBuilder,
    private opsService: ShipmentsService,
    private clientService: ClientService,
  ) {
    this.shipmentForm = this.fb.group({
      clientId: ['', Validators.required],
      receiverName: ['', Validators.required],
      receiverPhone: ['', Validators.required],
      destination: ['', Validators.required],
      items: this.fb.array([this.createItem()]),
    });
  }

  ngOnInit() {
    this.loadClients();
  }

  loadClients() {
    this.clientService.getAllClients().subscribe({
      next: (res) => {
        if (res.success) this.clients = res.data;
      },
      error: () => this.showError('Failed to fetch clients list'),
    });
  }

  createItem(): FormGroup {
    return this.fb.group({
      productName: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      price: [0, [Validators.required, Validators.min(0)]],
    });
  }

  get items() {
    return this.shipmentForm.get('items') as FormArray;
  }

  addItem() {
    this.items.push(this.createItem());
  }

  removeItem(index: number) {
    if (this.items.length > 1) this.items.removeAt(index);
  }

  get progressPercentage(): number {
    return (this.currentStep / 3) * 100;
  }

  isStepValid(): boolean {
    if (this.currentStep === 1) {
      return !!(this.shipmentForm.get('clientId')?.valid &&
        this.shipmentForm.get('receiverName')?.valid &&
        this.shipmentForm.get('destination')?.valid,
      this.shipmentForm.get('receiverPhone')?.valid);
    }
    if (this.currentStep === 2) {
      return this.items.valid && this.items.length > 0;
    }
    return true;
  }

  nextStep() {
    if (this.isStepValid()) {
      if (this.currentStep < 3) this.currentStep++;
    } else {
      this.showError('Please complete all required fields in this step');
    }
  }

  prevStep() {
    if (this.currentStep > 1) this.currentStep--;
  }

  getSelectedClientName(): string {
    const client = this.clients.find((c) => c.id === this.shipmentForm.value.clientId);
    return client ? client.name : 'Not Selected';
  }

  submit() {
    if (this.shipmentForm.invalid) return;

    this.opsService.createShipment(this.shipmentForm.value).subscribe({
      next: (res) => {
        if (res.success) {
          Swal.fire('Success', 'Shipment created and dispatched!', 'success');
          this.resetWizard();
        }
      },
      error: () => this.showError('Connection error during submission'),
    });
  }

  private resetWizard() {
    this.shipmentForm.reset({ items: [] });
    this.items.clear();
    this.items.push(this.createItem());
    this.currentStep = 1;
  }

  private showError(msg: string) {
    Swal.fire('Error', msg, 'error');
  }
}
