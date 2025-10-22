import React from 'react';
import { render, screen, waitFor } from '@testing-library/react';
import Dashboard from './Dashboard';
import { AuthContext } from '../context/AuthContext';
import * as UserService from '../services/UserService';

jest.mock('../services/UserService');

describe('Dashboard Component Tests', () => {
  const mockUsers = [
    {
      id: 1,
      name: 'John Student',
      email: 'student@test.com',
      role: 'Student',
      gender: 'Male',
      phonenumber: '1234567890',
    },
    {
      id: 2,
      name: 'Jane Teacher',
      email: 'teacher@test.com',
      role: 'Teacher',
      gender: 'Female',
      phonenumber: '0987654321',
    },
  ];

  beforeEach(() => {
    jest.clearAllMocks();
    UserService.getUsers.mockResolvedValue(mockUsers);
  });

  test('1. Should display Teacher role badge', async () => {
    const teacherUser = { role: 'Teacher', name: 'Test Teacher' };

    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText(/Role: Teacher/i)).toBeInTheDocument();
    });
  });

  test('2. Should display Student role badge', async () => {
    const studentUser = { role: 'Student', name: 'Test Student' };

    render(
      <AuthContext.Provider value={{ user: studentUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText(/Role: Student/i)).toBeInTheDocument();
    });
  });

  test('3. Teacher should see "Add New User" button', async () => {
    const teacherUser = { role: 'Teacher', name: 'Test Teacher' };

    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText(/➕ Add New User/i)).toBeInTheDocument();
    });
  });

  test('4. Student should NOT see "Add New User" button', async () => {
    const studentUser = { role: 'Student', name: 'Test Student' };

    render(
      <AuthContext.Provider value={{ user: studentUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.queryByText(/➕ Add New User/i)).not.toBeInTheDocument();
    });
  });

  test('5. Student should see read-only message', async () => {
    const studentUser = { role: 'Student', name: 'Test Student' };

    render(
      <AuthContext.Provider value={{ user: studentUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText(/You can only view users/i)).toBeInTheDocument();
    });
  });

  test('6. Should load and display users list', async () => {
    const teacherUser = { role: 'Teacher', name: 'Test Teacher' };

    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText('John Student')).toBeInTheDocument();
      expect(screen.getByText('Jane Teacher')).toBeInTheDocument();
      expect(screen.getByText('student@test.com')).toBeInTheDocument();
      expect(screen.getByText('teacher@test.com')).toBeInTheDocument();
    });
  });

  test('7. Should display correct user count', async () => {
    const teacherUser = { role: 'Teacher', name: 'Test Teacher' };

    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText(/Users List \(2\)/i)).toBeInTheDocument();
    });
  });

  test('8. Teacher should see Edit and Delete buttons', async () => {
    const teacherUser = { role: 'Teacher', name: 'Test Teacher' };

    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      const editButtons = screen.getAllByText(/✏️ Edit/i);
      const deleteButtons = screen.getAllByText(/🗑️ Delete/i);
      expect(editButtons.length).toBeGreaterThan(0);
      expect(deleteButtons.length).toBeGreaterThan(0);
    });
  });

  test('9. Student should NOT see Edit and Delete buttons', async () => {
    const studentUser = { role: 'Student', name: 'Test Student' };

    render(
      <AuthContext.Provider value={{ user: studentUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.queryByText(/✏️ Edit/i)).not.toBeInTheDocument();
      expect(screen.queryByText(/🗑️ Delete/i)).not.toBeInTheDocument();
    });
  });

  test('10. Should display "No users found" when list is empty', async () => {
    UserService.getUsers.mockResolvedValue([]);
    const teacherUser = { role: 'Teacher', name: 'Test Teacher' };

    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'fake-token' }}>
        <Dashboard />
      </AuthContext.Provider>
    );

    await waitFor(() => {
      expect(screen.getByText(/⚠️ No users found/i)).toBeInTheDocument();
    });
  });
});
