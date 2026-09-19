import { Injectable } from '@angular/core';
import { AddToCart, Cart, UpdateCartItemRequestDto } from '../models/cart-item.model';
import { HttpService } from '../http/http.service';

const ENDPOINT_CART = "Cart"

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private http: HttpService) {}

  getCart() {
    return this.http.get<Cart>(
      ENDPOINT_CART
    );
  }

  addToCart(payload: AddToCart) {
    return this.http.post<Cart>(
      ENDPOINT_CART,
      payload
    );
  }

  updateCartItem(productId: number, payload: UpdateCartItemRequestDto) {
    return this.http.put<Cart>(
      ENDPOINT_CART + "/" + productId,
      payload
    );
  }

  removeFromCart(productId: number) {
    return this.http.delete<Cart>(
      ENDPOINT_CART + "/" + productId,
    );
  }
}