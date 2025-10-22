import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';

interface LoginRequest {
  email: string;
  password: string;
}

interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  dateOfBirth: Date;
  designation: 'Teacher' | 'Student';
}

interface AuthResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5244/api/Auth';
  private currentUserSubject = new BehaviorSubject<any>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const token = localStorage.getItem('token');
    
    if (token) {
      const role = this.getRoleFromToken(token);
      const email = this.getEmailFromToken(token);
      this.currentUserSubject.next({ token, role, email });
    }
  }

  private parseJwt(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      return JSON.parse(jsonPayload);
    } catch (e) {
      return null;
    }
  }

  private getRoleFromToken(token: string): string | null {
    const decoded = this.parseJwt(token);
    if (!decoded) return null;
    
   
    return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] 
           || decoded['role'] 
           || null;
  }

  private getEmailFromToken(token: string): string | null {
    const decoded = this.parseJwt(token);
    if (!decoded) return null;
    
    return decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] 
           || decoded['email'] 
           || null;
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap(response => {
          localStorage.setItem('token', response.token);
          
          const role = this.getRoleFromToken(response.token);
          const userEmail = this.getEmailFromToken(response.token);
          
          this.currentUserSubject.next({ 
            token: response.token, 
            role: role, 
            email: userEmail 
          });
        })
      );
  }

  register(user: RegisterRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    const token = this.getToken();
    if (!token) return null;
    return this.getRoleFromToken(token);
  }

  isTeacher(): boolean {
    const role = this.getRole();
    console.log('Checking role:', role); 
    return role === 'Teacher';
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
