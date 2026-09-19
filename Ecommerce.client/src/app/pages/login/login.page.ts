import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import {
  IonContent,
  IonItem,
  IonInput,
  IonButton,
  IonIcon,
  IonText,
  IonSpinner,
  ToastController,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import { logInOutline, personOutline, lockClosedOutline } from 'ionicons/icons';

import { AuthService } from '../../core/services/auth.services';
import { LoginRequest } from '../../core/models/auth.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  imports: [
    IonContent,
    IonItem,
    IonInput,
    IonButton,
    IonIcon,
    IonText,
    IonSpinner,
    CommonModule,
    FormsModule,
  ],
})
export class LoginPage {
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastCtrl = inject(ToastController);

  credentials = signal<LoginRequest>({ name: '', password: '' });
  loading = signal(false);

  constructor() {
    addIcons({
      'log-in-outline': logInOutline,
      'person-outline': personOutline,
      'lock-outline': lockClosedOutline,
    });
  }

  login() {
    const creds = this.credentials();
    if (!creds.name.trim() || !creds.password.trim()) {
      this.presentToast('Completa todos los campos', 'warning');
      return;
    }

    this.loading.set(true);
    this.authService.login(creds).subscribe({
      next: (res) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('name', res.name);
        localStorage.setItem('email', res.token);

        this.loading.set(false);
        this.presentToast('Bienvenido', 'success');
        this.router.navigate(['/tabs/products']);
      },
      error: () => {
        this.loading.set(false);
        this.presentToast('Credenciales incorrectas', 'danger');
      },
    });
  }

  goToRegister() {
    this.router.navigate(['/register']);
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
