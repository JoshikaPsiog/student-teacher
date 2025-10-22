import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import Register from './Register';
import * as authService from '../services/auth';

jest.mock('../services/auth');

const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockNavigate,
}));

// Mock window.alert
global.alert = jest.fn();

describe('Register Component Tests', () => {
  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('1. Should render registration form with all fields', () => {
    render(
      <BrowserRouter>
        <Register />
      </BrowserRouter>
    );

    expect(screen.getByText(/✨ Create Account/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Enter your full name/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/example@email.com/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Create a strong password/i)).toBeInTheDocument();
  });

  test('2. Should allow user to fill all form fields', () => {
    render(
      <BrowserRouter>
        <Register />
      </BrowserRouter>
    );

    const nameInput = screen.getByPlaceholderText(/Enter your full name/i);
    const emailInput = screen.getByPlaceholderText(/example@email.com/i);
    const passwordInput = screen.getByPlaceholderText(/Create a strong password/i);

    fireEvent.change(nameInput, { target: { value: 'John Doe' } });
    fireEvent.change(emailInput, { target: { value: 'john@example.com' } });
    fireEvent.change(passwordInput, { target: { value: 'Password123' } });

    expect(nameInput.value).toBe('John Doe');
    expect(emailInput.value).toBe('john@example.com');
    expect(passwordInput.value).toBe('Password123');
  });

  test('3. Should show role selection (Student/Teacher)', () => {
    render(
      <BrowserRouter>
        <Register />
      </BrowserRouter>
    );

    const roleSelect = screen.getByLabelText(/💼 Role/i);
    expect(roleSelect).toBeInTheDocument();
    expect(screen.getByText('Student')).toBeInTheDocument();
    expect(screen.getByText('Teacher')).toBeInTheDocument();
  });

  test('4. Should show gender selection', () => {
    render(
      <BrowserRouter>
        <Register />
      </BrowserRouter>
    );

    const genderSelect = screen.getByLabelText(/⚥ Gender/i);
    expect(genderSelect).toBeInTheDocument();
  });

  test('5. Should successfully register and navigate to login', async () => {
    authService.register.mockResolvedValue({ message: 'Registration successful' });

    render(
      <BrowserRouter>
        <Register />
      </BrowserRouter>
    );

    fireEvent.change(screen.getByPlaceholderText(/Enter your full name/i), {
      target: { value: 'John Doe' },
    });
    fireEvent.change(screen.getByPlaceholderText(/example@email.com/i), {
      target: { value: 'john@example.com' },
    });
    fireEvent.change(screen.getByPlaceholderText(/Create a strong password/i), {
      target: { value: 'Password123' },
    });

    const dobInput = screen.getByLabelText(/📅 Date of Birth/i);
    fireEvent.change(dobInput, { target: { value: '2000-01-15' } });

    fireEvent.click(screen.getByRole('button', { name: /Register/i }));

    await waitFor(() => {
      expect(global.alert).toHaveBeenCalledWith('✅ Registration successful! Please login.');
      expect(mockNavigate).toHaveBeenCalledWith('/login');
    });
  });
});
