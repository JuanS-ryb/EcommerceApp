export interface LoginRequest {
  name: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  email: string;
  name: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
}

export interface RegisterResponse {
  id: number;
  name: string;
  email: string;
  passwordHash: string;
  createdAt: string;
}