export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  stock: number;
  createdAt: string;
}

export interface CreateUpdateProductoDto {
  name: string;
  description?: string | null;
  price: number;
  ImageUrl?: string | null;
  Stock: number;
}
