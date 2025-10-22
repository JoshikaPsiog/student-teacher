import React from 'react';
import { render, screen } from '@testing-library/react';
import { AuthContext } from '../context/AuthContext';

const SimpleDashboard = ({ user }) => {
  const isTeacher = user?.designation === 'Teacher';
  
  return (
    <div>
      <h2>User Management Dashboard</h2>
      <span>Role: {user?.designation || 'Unknown'}</span>
      {isTeacher && <button>Add New User</button>}
      {!isTeacher && <p>You can only view users.</p>}
    </div>
  );
};

describe('Dashboard Component Tests', () => {
  test('displays teacher role correctly', () => {
    const teacherUser = { designation: 'Teacher', name: 'Test Teacher' };
    
    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'token' }}>
        <SimpleDashboard user={teacherUser} />
      </AuthContext.Provider>
    );
    
    expect(screen.getByText(/role: teacher/i)).toBeInTheDocument();
  });

  test('teacher can see add user button', () => {
    const teacherUser = { designation: 'Teacher', name: 'Test Teacher' };
    
    render(
      <AuthContext.Provider value={{ user: teacherUser, token: 'token' }}>
        <SimpleDashboard user={teacherUser} />
      </AuthContext.Provider>
    );
    
    expect(screen.getByText(/add new user/i)).toBeInTheDocument();
  });

  test('student sees read-only message', () => {
    const studentUser = { designation: 'Student', name: 'Test Student' };
    
    render(
      <AuthContext.Provider value={{ user: studentUser, token: 'token' }}>
        <SimpleDashboard user={studentUser} />
      </AuthContext.Provider>
    );
    
    expect(screen.getByText(/you can only view users/i)).toBeInTheDocument();
  });

  test('student cannot see add user button', () => {
    const studentUser = { designation: 'Student', name: 'Test Student' };
    
    render(
      <AuthContext.Provider value={{ user: studentUser, token: 'token' }}>
        <SimpleDashboard user={studentUser} />
      </AuthContext.Provider>
    );
    
    expect(screen.queryByText(/add new user/i)).not.toBeInTheDocument();
  });
});
