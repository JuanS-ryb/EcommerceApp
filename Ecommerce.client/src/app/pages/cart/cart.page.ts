import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonButton,
  IonIcon,
  IonItem,
  IonLabel,
  IonList,
  IonThumbnail,
  IonText,
  IonSpinner,
  IonNote,
  IonFooter,
  ToastController,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import {
  trashOutline,
  removeCircleOutline,
  addCircleOutline,
  cartOutline,
  chevronForwardOutline,
} from 'ionicons/icons';

import { CartService } from '../../core/services/cart.services';
import { Cart, CartItem } from '../../core/models/cart-item.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.page.html',
  styleUrls: ['./cart.page.scss'],
  imports: [
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonButton,
    IonIcon,
    IonItem,
    IonLabel,
    IonList,
    IonThumbnail,
    IonText,
    IonSpinner,
    IonNote,
    IonFooter,
    CommonModule,
    FormsModule,
  ],
})
export class CartPage implements OnInit {
  private cartService = inject(CartService);
  private router = inject(Router);
  private toastCtrl = inject(ToastController);

  cart = signal<Cart | null>(null);
  loading = signal(false);

  constructor() {
    addIcons({
      'trash-outline': trashOutline,
      'remove-circle-outline': removeCircleOutline,
      'add-circle-outline': addCircleOutline,
      'cart-outline': cartOutline,
      'chevron-forward-outline': chevronForwardOutline,
    });
  }

  ngOnInit() {}

  ionViewWillEnter() {
    this.loadCart();
  }

  loadCart() {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/login']);
      return;
    }
    this.loading.set(true);
    this.cartService.getCart().subscribe({
      next: (data) => {
        this.cart.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.presentToast('Error al cargar el carrito', 'danger');
      },
    });
  }

  increaseQuantity(item: CartItem) {
    this.cartService
      .updateCartItem(item.productId, { quantity: item.quantity + 1 })
      .subscribe({
        next: (data) => this.cart.set(data),
        error: () => this.presentToast('Error al actualizar', 'danger'),
      });
  }

  decreaseQuantity(item: CartItem) {
    if (item.quantity <= 1) {
      this.removeItem(item);
      return;
    }
    this.cartService
      .updateCartItem(item.productId, { quantity: item.quantity - 1 })
      .subscribe({
        next: (data) => this.cart.set(data),
        error: () => this.presentToast('Error al actualizar', 'danger'),
      });
  }

  removeItem(item: CartItem) {
    this.cartService.removeFromCart(item.productId).subscribe({
      next: (data) => {
        this.cart.set(data);
        this.presentToast('Producto eliminado', 'success');
      },
      error: () => this.presentToast('Error al eliminar', 'danger'),
    });
  }

  goToCheckout() {
    this.router.navigate(['/checkout']);
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
