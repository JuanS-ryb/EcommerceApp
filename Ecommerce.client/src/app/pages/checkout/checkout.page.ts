import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonButtons,
  IonBackButton,
  IonButton,
  IonIcon,
  IonItem,
  IonLabel,
  IonInput,
  IonText,
  IonSpinner,
  IonTextarea,
  IonFooter,
  ToastController,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import {
  checkmarkCircleOutline,
  locationOutline,
  cardOutline,
} from 'ionicons/icons';

import { CartService } from '../../core/services/cart.services';
import { OrderService } from '../../core/services/order.services';
import { Cart } from '../../core/models/cart-item.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.page.html',
  styleUrls: ['./checkout.page.scss'],
  imports: [
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonButtons,
    IonBackButton,
    IonButton,
    IonIcon,
    IonItem,
    IonLabel,
    IonInput,
    IonText,
    IonSpinner,
    IonTextarea,
    IonFooter,
    CommonModule,
    FormsModule,
  ],
})
export class CheckoutPage implements OnInit {
  private cartService = inject(CartService);
  private orderService = inject(OrderService);
  private router = inject(Router);
  private toastCtrl = inject(ToastController);

  cart = signal<Cart | null>(null);
  loading = signal(false);
  processing = signal(false);
  shippingAddress = signal('');
  paymentMethod = signal('');

  constructor() {
    addIcons({
      'checkmark-circle-outline': checkmarkCircleOutline,
      'location-outline': locationOutline,
      'card-outline': cardOutline,
    });
  }

  ngOnInit() {
    this.loadCart();
  }

  loadCart() {
    this.loading.set(true);
    this.cartService.getCart().subscribe({
      next: (data) => {
        this.cart.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  placeOrder() {
    if (!this.shippingAddress().trim()) {
      this.presentToast('Ingresa una dirección de envío', 'warning');
      return;
    }

    if (!this.paymentMethod().trim()) {
      this.presentToast('Ingresa un metodo de pago valido', 'warning');
      return;
    }

    this.processing.set(true);
    this.orderService.checkout({
      address: this.shippingAddress(),
      paymentMethod: this.paymentMethod()
    }).subscribe({
      next: () => {
        this.processing.set(false);
        this.presentToast('Pedido realizado con éxito', 'success');
        this.router.navigate(['/tabs/orders']);
      },
      error: () => {
        this.processing.set(false);
        this.presentToast('Error al procesar el pedido', 'danger');
      },
    });
  }

  private async presentToast(message: string, color: string) {
    const toast = await this.toastCtrl.create({
      message,
      duration: 2000,
      color,
      position: 'bottom',
    });
    await toast.present();
  }
}
