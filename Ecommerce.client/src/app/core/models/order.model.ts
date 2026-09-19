export interface OrderItem {
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

export interface Order {
  id: number;
  orderDate: string;
  total: number;
  status: string;
  items: OrderItem[];
}

export interface CreateOrder {
  address: string,
  paymentMethod: string
}