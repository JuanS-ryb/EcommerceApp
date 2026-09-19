import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonSearchbar,
  IonButtons,
  IonButton,
  IonIcon,
  IonBadge,
  IonCard,
  IonCardHeader,
  IonCardTitle,
  IonCardSubtitle,
  IonCardContent,
  IonSpinner,
  IonText,
  ToastController,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import {
  cartOutline,
  searchOutline,
  addCircle,
  logInOutline,
} from 'ionicons/icons';

import { ProductService } from '../../core/services/product.services';
import { CartService } from '../../core/services/cart.services';
import { Product } from '../../core/models/product.model';

@Component({
  selector: 'app-products',
  templateUrl: './products.page.html',
  styleUrls: ['./products.page.scss'],
  imports: [
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonSearchbar,
    IonButtons,
    IonButton,
    IonIcon,
    IonBadge,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardSubtitle,
    IonCardContent,
    IonSpinner,
    IonText,
    CommonModule,
    FormsModule,
  ],
})
export class ProductsPage implements OnInit {
  private productService = inject(ProductService);
  private cartService = inject(CartService);
  private router = inject(Router);
  private toastCtrl = inject(ToastController);

  products = signal<Product[]>([]);
  loading = signal(false);
  searchTerm = signal('');
  cartCount = signal(0);
  isLoggedIn = signal(false);

  filteredProducts = computed(() => this.products());

  constructor() {
    addIcons({
      'cart-outline': cartOutline,
      'search-outline': searchOutline,
      'add-circle': addCircle,
      'log-in-outline': logInOutline,
    });
  }

  ngOnInit() {
    this.loadProducts();
  }

  ionViewWillEnter() {
    this.isLoggedIn.set(!!localStorage.getItem('token'));
    this.loadCartCount();
  }

  loadCartCount() {
    if (!localStorage.getItem('token')) {
      this.cartCount.set(0);
      return;
    }
    this.cartService.getCart().subscribe({
      next: (data) => {
        this.cartCount.set(data?.items?.length ?? 0);
      },
      error: () => {
        this.cartCount.set(0);
      },
    });
  }

  loadProducts() {
    this.loading.set(true);
    this.productService.getProducts(null).subscribe({
      next: (data) => {
        this.products.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.presentToast('Error al cargar productos', 'danger');
      },
    });
  }

  onSearch(event: any) {
    const term = event.detail.value || '';
    this.searchTerm.set(term);
    this.loading.set(true);
    this.productService.getProducts(term.trim() || null).subscribe({
      next: (data) => {
        this.products.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  addToCart(product: Product) {
    if (!localStorage.getItem('token')) {
      this.router.navigate(['/login']);
      return;
    }
    this.cartService
      .addToCart({ productId: product.id, quantity: 1 })
      .subscribe({
        next: (cart) => {
          this.cartCount.set(cart?.items?.length ?? 0);
          this.presentToast(`${product.name} agregado al carrito`, 'success');
        },
        error: () => {
          this.presentToast('Error al agregar al carrito', 'danger');
        },
      });
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  goToCart() {
    this.router.navigate(['/tabs/cart']);
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
