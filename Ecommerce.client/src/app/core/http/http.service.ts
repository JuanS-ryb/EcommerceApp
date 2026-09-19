import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

const PORT = 7002;
const BASE_URL = `https://localhost:${PORT}/api`;

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  constructor(private http: HttpClient) {}

  get<T>(url: string, params?: Record<string, string | number | null>) {
    let httpParams = new HttpParams();

    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (value !== null) {
          httpParams = httpParams.set(key, value.toString());
        }
      });
    }

    return this.http.get<T>(`${BASE_URL}/${url}`, { params: httpParams });
  }

  post<T>(url: string, body: unknown) {
    return this.http.post<T>(`${BASE_URL}/${url}`, body);
  }

  put<T>(url: string, body: unknown) {
    return this.http.put<T>(`${BASE_URL}/${url}`, body);
  }

  delete<T>(url: string) {
    return this.http.delete<T>(`${BASE_URL}/${url}`);
  }
}
