import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonItem,
  IonLabel,
  IonText,
  IonIcon,
  IonBadge,
  IonSpinner,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import { receiptOutline } from 'ionicons/icons';

import { OrderService } from '../../core/services/order.services';
import { Order } from '../../core/models/order.model';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.page.html',
  styleUrls: ['./orders.page.scss'],
  imports: [
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonItem,
    IonLabel,
    IonText,
    IonIcon,
    IonBadge,
    IonSpinner,
    CommonModule,
  ],
})
export class OrdersPage implements OnInit {
  private orderService = inject(OrderService);
  private router = inject(Router);

  orders = signal<Order[]>([]);
  loading = signal(false);

  constructor() {
    addIcons({ 'receipt-outline': receiptOutline });
  }

  ngOnInit() {}

  ionViewWillEnter() {
    this.loadOrders();
  }

  loadOrders() {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/login']);
      return;
    }
    this.loading.set(true);
    this.orderService.getMyOrders().subscribe({
      next: (data) => {
        this.orders.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'pendiente':
        return 'warning';
      case 'completado':
        return 'success';
      case 'cancelado':
        return 'danger';
      default:
        return 'medium';
    }
  }
}
