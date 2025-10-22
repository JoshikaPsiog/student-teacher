import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserService, User } from '../services/user.service';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  errorMsg = '';
  isTeacher = false;
  loading = false;

  showForm = false;
  isEditMode = false;
  currentUser: User = this.getEmptyUser();

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.isTeacher = this.authService.isTeacher();
    this.loadUsers();
  }

  loadUsers(): void {
    console.log('Loading users...');
    console.log('Token:', this.authService.getToken());
    console.log('Is Teacher:', this.isTeacher);
    
    this.loading = true;
    this.userService.getAllUsers().subscribe({
      next: (data) => {
        console.log('Users loaded:', data);
        this.users = data;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading users:', error);
        this.errorMsg = 'Failed to load users';
        this.loading = false;
      }
    });
  }

  getEmptyUser(): User {
    const today = new Date();
    const dateString = today.toISOString().split('T')[0]; 
    
    return {
      name: '',
      dateOfBirth: dateString,
      designation: 'Student',
      email: '',
      password: ''
    };
  }

  openCreateForm(): void {
    if (!this.isTeacher) {
      this.errorMsg = 'Only teachers can create users.';
      return;
    }
    this.isEditMode = false;
    this.currentUser = this.getEmptyUser();
    this.showForm = true;
    this.errorMsg = '';
  }

  openEditForm(user: User): void {
    if (!this.isTeacher) {
      this.errorMsg = 'Only teachers can edit users.';
      return;
    }
    this.isEditMode = true;
    

    const userCopy = { ...user };
    if (userCopy.dateOfBirth) {
      const date = new Date(userCopy.dateOfBirth);
      userCopy.dateOfBirth = date.toISOString().split('T')[0];
    }
    
    this.currentUser = userCopy;
    this.showForm = true;
    this.errorMsg = '';
  }

 saveUser(): void {
  if (!this.isTeacher) {
    this.errorMsg = 'Only teachers can save users.';
    return;
  }

 
  if (!this.currentUser.name || !this.currentUser.email || !this.currentUser.dateOfBirth) {
    this.errorMsg = 'Please fill all required fields';
    return;
  }


  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!emailRegex.test(this.currentUser.email)) {
    this.errorMsg = 'Please enter a valid email address';
    return;
  }

  this.loading = true;
  this.errorMsg = '';

  if (this.isEditMode && this.currentUser.userId) {
 
    const userToUpdate = { ...this.currentUser };
    delete userToUpdate.password;

    this.userService.updateUser(this.currentUser.userId, userToUpdate).subscribe({
      next: () => {
        this.loading = false;
        this.loadUsers();
        this.cancelForm();
      },
      error: (error) => {
        this.loading = false;
        this.errorMsg = error.error?.message || 'Failed to update user';
        console.error('Error updating user:', error);
      }
    });
  } else {
    
    const newUser = { ...this.currentUser };
    delete newUser.password; 

    this.userService.createUser(newUser).subscribe({
      next: () => {
        this.loading = false;
        this.loadUsers();
        this.cancelForm();
      },
      error: (error) => {
        this.loading = false;
        this.errorMsg = error.error?.message || 'Failed to create user';
        console.error('Error creating user:', error);
      }
    });
  }
}

  deleteUser(userId: number): void {
    if (!this.isTeacher) {
      this.errorMsg = 'Only teachers can delete users.';
      return;
    }

    if (confirm('Are you sure you want to delete this user?')) {
      this.userService.deleteUser(userId).subscribe({
        next: () => {
          this.loadUsers();
          this.errorMsg = '';
        },
        error: (error) => {
          this.errorMsg = 'Failed to delete user';
          console.error('Error deleting user:', error);
        }
      });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  cancelForm(): void {
    this.showForm = false;
    this.errorMsg = '';
    this.currentUser = this.getEmptyUser();
  }
}
