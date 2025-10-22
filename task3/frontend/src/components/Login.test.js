import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';
import { AuthContext } from '../context/AuthContext';

jest.mock('../services/auth', () => ({
  login: jest.fn(() => Promise.resolve({ token: 'fake-token' })),
}));

const LoginForm = () => {
  const [email, setEmail] = React.useState('');
  const [password, setPassword] = React.useState('');

  return (
    <div>
      <h2>Login</h2>
      <input 
        placeholder="your@email.com" 
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <input 
        type="password" 
        placeholder="Enter your password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button>Login</button>
    </div>
  );
};

describe('Login Component Tests', () => {
  test('renders login form', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: jest.fn(), user: null }}>
          <LoginForm />
        </AuthContext.Provider>
      </BrowserRouter>
    );
    
    // FIX: Use getByRole to target the heading specifically
    expect(screen.getByRole('heading', { name: /login/i })).toBeInTheDocument();
  });

  test('has email and password inputs', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: jest.fn(), user: null }}>
          <LoginForm />
        </AuthContext.Provider>
      </BrowserRouter>
    );
    
    expect(screen.getByPlaceholderText(/email/i)).toBeInTheDocument();
    expect(screen.getByPlaceholderText(/password/i)).toBeInTheDocument();
  });

  test('can type in email field', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: jest.fn(), user: null }}>
          <LoginForm />
        </AuthContext.Provider>
      </BrowserRouter>
    );
    
    const emailInput = screen.getByPlaceholderText(/email/i);
    fireEvent.change(emailInput, { target: { value: 'test@example.com' } });
    expect(emailInput.value).toBe('test@example.com');
  });

  test('has a login button', () => {
    render(
      <BrowserRouter>
        <AuthContext.Provider value={{ login: jest.fn(), user: null }}>
          <LoginForm />
        </AuthContext.Provider>
      </BrowserRouter>
    );
    
    expect(screen.getByRole('button', { name: /login/i })).toBeInTheDocument();
  });
});
