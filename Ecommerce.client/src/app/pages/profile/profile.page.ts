import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import {
  IonContent,
  IonHeader,
  IonTitle,
  IonToolbar,
  IonItem,
  IonLabel,
  IonButton,
  IonIcon,
  IonText,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import {
  personCircleOutline,
  logOutOutline,
  cartOutline,
  receiptOutline,
} from 'ionicons/icons';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.page.html',
  styleUrls: ['./profile.page.scss'],
  imports: [
    IonContent,
    IonHeader,
    IonTitle,
    IonToolbar,
    IonItem,
    IonLabel,
    IonButton,
    IonIcon,
    IonText,
    CommonModule,
  ],
})
export class ProfilePage {
  router = inject(Router);

  isLoggedIn = signal(false);
  userName = signal('');

  constructor() {
    addIcons({
      'person-circle-outline': personCircleOutline,
      'log-out-outline': logOutOutline,
      'cart-outline': cartOutline,
      'receipt-outline': receiptOutline,
    });
  }

  ionViewWillEnter() {
    const token = localStorage.getItem('token');
    this.isLoggedIn.set(!!token);
    if (token) {
      this.userName.set(localStorage.getItem('name') || 'Usuario');
    }
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userName');
    this.router.navigate(['/login']);
  }

  goToLogin() {
    this.router.navigate(['/login']);
  }

  goToRegister() {
    this.router.navigate(['/register']);
  }
}
