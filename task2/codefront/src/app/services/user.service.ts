import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

export interface User {
  userId?: number;
  name: string;
  dateOfBirth: string | Date;  
  designation: 'Teacher' | 'Student';
  email: string;
  password?: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5244/api/Users';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.apiUrl, { headers: this.getHeaders() });
  }

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }

  createUser(user: User): Observable<User> {
    
    const userData = {
      ...user,
      dateOfBirth: user.dateOfBirth instanceof Date 
        ? user.dateOfBirth.toISOString().split('T')[0]  // Format as YYYY-MM-DD
        : user.dateOfBirth
    };
    return this.http.post<User>(this.apiUrl, userData, { headers: this.getHeaders() });
  }

  updateUser(id: number, user: User): Observable<any> {
    const userData = {
      ...user,
      dateOfBirth: user.dateOfBirth instanceof Date 
        ? user.dateOfBirth.toISOString().split('T')[0]
        : user.dateOfBirth
    };
    return this.http.put(`${this.apiUrl}/${id}`, userData, { headers: this.getHeaders() });
  }

  deleteUser(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`, { headers: this.getHeaders() });
  }
}
