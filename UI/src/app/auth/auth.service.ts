import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoginInformation } from './login/login.model';
import { RegisterInformation } from './register/register.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:8071/api/account';
  private tokenKey = 'authToken';

  constructor(private http: HttpClient) {}

  login(credentials: LoginInformation): Observable<any> {
    return this.http
      .post<{ token: string }>(`${this.apiUrl}/login`, credentials)
      .pipe(tap((response) => this.setToken(response.token)));
  }

  register(user: RegisterInformation): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    return token != null;
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.http.get(`${this.apiUrl}/logout`).subscribe();
  }
}
