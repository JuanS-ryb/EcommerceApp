import { Injectable, signal } from '@angular/core';

import {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse
} from '../models/auth.model';
import { HttpService } from '../http/http.service';

const ENDPOINT_LOGIN = "Auth/login";
const ENDPOINT_REGISTER = "Auth/register";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUser = signal<LoginResponse | null>(null);

  constructor(private http: HttpService) {}

  login(payload: LoginRequest) {
    return this.http.post<LoginResponse>(
      ENDPOINT_LOGIN,
      payload
    );
  }

  register(payload: RegisterRequest) {
    return this.http.post<RegisterResponse>(
      ENDPOINT_REGISTER,
      payload
    );
  }
}