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
import {
  personOutline,
  mailOutline,
  lockClosedOutline,
  personAddOutline,
} from 'ionicons/icons';

import { AuthService } from '../../core/services/auth.services';
import { RegisterRequest } from '../../core/models/auth.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.page.html',
  styleUrls: ['./register.page.scss'],
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
export class RegisterPage {
  private authService = inject(AuthService);
  private router = inject(Router);
  private toastCtrl = inject(ToastController);

  user = signal<RegisterRequest>({ name: '', email: '', password: '' });
  loading = signal(false);

  constructor() {
    addIcons({
      'person-outline': personOutline,
      'mail-outline': mailOutline,
      'lock-outline': lockClosedOutline,
      'person-add-outline': personAddOutline,
    });
  }

  register() {
    const u = this.user();
    if (!u.name.trim() || !u.email.trim() || !u.password.trim()) {
      this.presentToast('Completa todos los campos', 'warning');
      return;
    }
    if (!u.email.includes('@')) {
      this.presentToast('Ingresa un correo válido', 'warning');
      return;
    }

    this.loading.set(true);
    this.authService.register(u).subscribe({
      next: () => {
        this.loading.set(false);
        this.presentToast('Cuenta creada exitosamente', 'success');
        this.router.navigate(['/login']);
      },
      error: () => {
        this.loading.set(false);
        this.presentToast('Error al registrar', 'danger');
      },
    });
  }

  goToLogin() {
    this.router.navigate(['/login']);
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
