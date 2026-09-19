import { Component } from '@angular/core';
import {
  IonTabs,
  IonTabBar,
  IonTabButton,
  IonIcon,
  IonLabel,
} from '@ionic/angular';
import { addIcons } from 'ionicons';
import {
  storefrontOutline,
  cartOutline,
  receiptOutline,
  personCircleOutline,
} from 'ionicons/icons';

@Component({
  selector: 'app-tabs',
  template: `
    <ion-tabs>
      <ion-tab-bar slot="bottom">
        <ion-tab-button tab="products">
          <ion-icon aria-hidden="true" name="storefront-outline"></ion-icon>
          <ion-label>Productos</ion-label>
        </ion-tab-button>
        <ion-tab-button tab="cart">
          <ion-icon aria-hidden="true" name="cart-outline"></ion-icon>
          <ion-label>Carrito</ion-label>
        </ion-tab-button>
        <ion-tab-button tab="orders">
          <ion-icon aria-hidden="true" name="receipt-outline"></ion-icon>
          <ion-label>Pedidos</ion-label>
        </ion-tab-button>
        <ion-tab-button tab="profile">
          <ion-icon aria-hidden="true" name="person-circle-outline"></ion-icon>
          <ion-label>Perfil</ion-label>
        </ion-tab-button>
      </ion-tab-bar>
    </ion-tabs>
  `,
  imports: [IonTabs, IonTabBar, IonTabButton, IonIcon, IonLabel],
})
export class TabsPage {
  constructor() {
    addIcons({
      'storefront-outline': storefrontOutline,
      'cart-outline': cartOutline,
      'receipt-outline': receiptOutline,
      'person-circle-outline': personCircleOutline,
    });
  }
}
