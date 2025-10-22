import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import Login from './Login';
import { AuthContext } from '../context/AuthContext';
import * as authService from '../services/auth';

// Mock the auth service
jest.mock('../services/auth');

// Mock navigate
const mockNavigate = jest.fn();
jest.mock('react-router-dom', () => ({
  ...jest.requireActual('react-router-dom'),
  useNavigate: () => mockNavigate,
}));

describe('Login Component Tests', () => {
  const mockLogin = jest.fn();

  beforeEach(() => {
    jest.clearAllMocks();
  });

  test('1. Should render login form with all elements', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: mockLogin, user: null }}>
          <Login />
        </AuthContext.Provider>
      </BrowserRouter>
    );

    expect(screen.getByText(/🔒 Login/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/your@email.com/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/Enter your password/i)).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /Login/i })).toBeInTheDocument();
  });

  test('2. Should allow user to type in email field', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: mockLogin, user: null }}>
          <Login />
        </AuthContext.Provider>
      </BrowserRouter>
    );

    const emailInput = screen.getByPlaceholderText(/your@email.com/i);
    fireEvent.change(emailInput, { target: { value: 'test@example.com' } });
    expect(emailInput.value).toBe('test@example.com');
  });

  test('3. Should allow user to type in password field', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: mockLogin, user: null }}>
          <Login />
        </AuthContext.Provider>
      </BrowserRouter>
    );

    const passwordInput = screen.getByPlaceholderText(/Enter your password/i);
    fireEvent.change(passwordInput, { target: { value: 'password123' } });
    expect(passwordInput.value).toBe('password123');
  });

  test('4. Should show loading state when submitting', async () => {
    authService.login.mockImplementation(() => new Promise(() => {})); // Never resolves

    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: mockLogin, user: null }}>
          <Login />
        </AuthContext.Provider>
      </BrowserRouter>
    );

    fireEvent.change(screen.getByPlaceholderText(/your@email.com/i), {
      target: { value: 'test@example.com' },
    });
    fireEvent.change(screen.getByPlaceholderText(/Enter your password/i), {
      target: { value: 'password123' },
    });
    fireEvent.click(screen.getByRole('button', { name: /Login/i }));

    await waitFor(() => {
      expect(screen.getByText(/Logging in.../i)).toBeInTheDocument();
    });
  });

  test('5. Should call login and navigate on successful login', async () => {
    authService.login.mockResolvedValue({ token: 'fake-jwt-token' });

    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: mockLogin, user: null }}>
          <Login />
        </AuthContext.Provider>
      </BrowserRouter>
    );

    fireEvent.change(screen.getByPlaceholderText(/your@email.com/i), {
      target: { value: 'test@example.com' },
    });
    fireEvent.change(screen.getByPlaceholderText(/Enter your password/i), {
      target: { value: 'password123' },
    });
    fireEvent.click(screen.getByRole('button', { name: /Login/i }));

    await waitFor(() => {
      expect(mockLogin).toHaveBeenCalledWith('fake-jwt-token');
      expect(mockNavigate).toHaveBeenCalledWith('/dashboard');
    });
  });
});
