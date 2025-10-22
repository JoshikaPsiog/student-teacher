import React, { createContext, useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [user, setUser] = useState(null);

  useEffect(() => {
    if (token) {
      try {
        const decoded = jwtDecode(token);
        
        const roleClaimKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
        const nameIdClaimKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
        const nameClaimKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
        const emailClaimKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';
        
        setUser({
          userId: decoded[nameIdClaimKey] || decoded.nameid || decoded.sub,
          name: decoded[nameClaimKey] || decoded.name || decoded.unique_name,
          email: decoded[emailClaimKey] || decoded.email,
          role: decoded[roleClaimKey] || decoded.role || 'Student'
        });
      } catch (error) {
        console.error('Invalid token:', error);
        logout();
      }
    } else {
      setUser(null);
    }
  }, [token]);

  function login(newToken) {
    localStorage.setItem('token', newToken);
    setToken(newToken);
  }

  function logout() {
    localStorage.removeItem('token');
    setToken(null);
    setUser(null);
  }

  return (
    <AuthContext.Provider value={{ token, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}
