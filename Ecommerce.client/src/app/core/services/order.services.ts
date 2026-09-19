import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateOrder, Order } from '../models/order.model';
import { HttpService } from '../http/http.service';

export const OrderEndpoint = {
  checkout: `Order/checkOut`,
  myOrders: `Order/getMyOrders`
};

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpService) {}

  checkout(body: CreateOrder) {
    return this.http.post<Order>(
      OrderEndpoint.checkout,
      body
    );
  }

  getMyOrders() {
    return this.http.get<Order[]>(
      OrderEndpoint.myOrders
    );
  } }