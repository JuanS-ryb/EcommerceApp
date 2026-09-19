import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { CreateUpdateProductoDto, Product } from '../models/product.model';
import { HttpService } from '../http/http.service';

const ENDPOINT_PRODUCTURL = "Producto"

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private http: HttpService) {}

  getProducts(term: string | null) {
    return this.http.get<Product[]>(
      ENDPOINT_PRODUCTURL,
      {
        term: term
      }
    );
  }

  getProductById(id: number) {
    return this.http.get<Product>(
      ENDPOINT_PRODUCTURL + "/" + id
    );
  }

  createProduct(payload: CreateUpdateProductoDto) {
    return this.http.post<Product>(
      ENDPOINT_PRODUCTURL,
      payload
    );
  }

  updateProduct(id: number, payload: CreateUpdateProductoDto) {
    return this.http.put<Product>(
      ENDPOINT_PRODUCTURL + "/" + id,
      payload
    );
  }
}