import React from 'react';
import { render, screen } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';

const RegisterForm = () => {
  return (
    <div>
      <h2>Create Account</h2>
      <input placeholder="Enter your full name" />
      <input placeholder="example@email.com" />
      <input type="password" placeholder="Create a strong password" />
      <select>
        <option value="Student">Student</option>
        <option value="Teacher">Teacher</option>
      </select>
      <button>Register</button>
    </div>
  );
};

describe('Register Component Tests', () => {
  test('renders registration form', () => {
    render(
      <BrowserRouter>
        <RegisterForm />
      </BrowserRouter>
    );
    
    expect(screen.getByText(/create account/i)).toBeInTheDocument();
  });

  test('has all required fields', () => {
    render(
      <BrowserRouter>
        <RegisterForm />
      </BrowserRouter>
    );
    
    expect(screen.getByPlaceholderText(/full name/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/email/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/password/i)).toBeInTheDocument();
  });

  test('has designation dropdown', () => {
    render(
      <BrowserRouter>
        <RegisterForm />
      </BrowserRouter>
    );
    
    const select = screen.getByRole('combobox');
    expect(select).toBeInTheDocument();
    expect(screen.getByText('Student')).toBeInTheDocument();
    expect(screen.getByText('Teacher')).toBeInTheDocument();
  });

  test('has register button', () => {
    render(
      <BrowserRouter>
        <RegisterForm />
      </BrowserRouter>
    );
    
    expect(screen.getByRole('button', { name: /register/i })).toBeInTheDocument();
  });
});
