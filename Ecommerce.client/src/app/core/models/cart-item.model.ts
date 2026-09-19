import { Product } from './product.model';

export interface CartItem {
  productId: number;
  product: Product;
  quantity: number;
  subtotal: number;
}

export interface Cart {
  items: CartItem[];
  total: number;
}

export interface AddToCart extends UpdateCartItemRequestDto { 
  productId: number;
}

export interface UpdateCartItemRequestDto {
  quantity: number;
}