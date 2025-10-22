import React, { useContext, useEffect, useState } from 'react';
import { AuthContext } from '../context/AuthContext';
import { getUsers, createUser, updateUser, deleteUser } from '../services/UserService';
import UserForm from './UserForm';
import UserList from './UserList';

export default function Dashboard() {
  const { token, user } = useContext(AuthContext);
  const [users, setUsers] = useState([]);
  const [editing, setEditing] = useState(null);
  const [error, setError] = useState('');
  const [showForm, setShowForm] = useState(false);

  const isTeacher = user?.role === 'Teacher';

  useEffect(() => {
    if (token) load();
  }, [token]);

  async function load() {
    try {
      const data = await getUsers(token);
      setUsers(data);
      setError('');
    } catch (err) {
      console.error('Load error:', err);
      setError(err.response?.data?.message || 'Failed to load users');
    }
  }

  async function handleSave(payload) {
    if (!isTeacher) {
      setError('Only Teachers can create/update users');
      return;
    }

    try {
      if (editing) {
        await updateUser(token, editing.id, payload);
      } else {
        await createUser(token, payload);
      }
      setEditing(null);
      setShowForm(false);
      await load();
    } catch (err) {
      console.error('Save error:', err);
      setError(err.response?.data?.message || err.message || 'Save failed');
    }
  }

  async function handleDelete(id) {
    if (!isTeacher) {
      setError('Only Teachers can delete users');
      return;
    }

    if (!window.confirm('Delete this user?')) return;

    try {
      await deleteUser(token, id);
      await load();
    } catch (err) {
      console.error('Delete error:', err);
      setError(err.response?.data?.message || 'Delete failed');
    }
  }

  function handleEdit(user) {
    if (!isTeacher) {
      setError('Only Teachers can edit users');
      return;
    }
    setEditing(user);
    setShowForm(true);
  }

  return (
    <div style={{ maxWidth: '1200px', margin: '0 auto', padding: '20px' }}>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '20px' }}>
        <h2>👥 User Management Dashboard</h2>
        <div>
          <span style={{
            padding: '8px 15px',
            backgroundColor: '#17a2b8',
            color: 'white',
            borderRadius: '20px',
            marginRight: '10px',
            fontSize: '14px',
            fontWeight: 'bold'
          }}>
            Role: {user?.role || 'Unknown'}
          </span>
          {isTeacher && !showForm && (
            <button 
              onClick={() => setShowForm(true)}
              style={{
                padding: '10px 20px',
                backgroundColor: '#28a745',
                color: 'white',
                border: 'none',
                borderRadius: '5px',
                cursor: 'pointer',
                fontWeight: 'bold'
              }}
            >
              ➕ Add New User
            </button>
          )}
        </div>
      </div>

      {error && (
        <div style={{
          padding: '12px',
          backgroundColor: '#f8d7da',
          color: '#721c24',
          borderRadius: '5px',
          marginBottom: '15px',
          border: '1px solid #f5c6cb'
        }}>
          ⚠️ {error}
          <button 
            onClick={() => setError('')}
            style={{
              float: 'right',
              background: 'none',
              border: 'none',
              fontSize: '20px',
              cursor: 'pointer',
              color: '#721c24'
            }}
          >
            ×
          </button>
        </div>
      )}

      {!isTeacher && (
        <div style={{
          padding: '15px',
          backgroundColor: '#d1ecf1',
          color: '#0c5460',
          borderRadius: '5px',
          marginBottom: '15px',
          border: '1px solid #bee5eb'
        }}>
          ℹ️ You are logged in as a <strong>Student</strong>. You can only view users.
        </div>
      )}

      {showForm && isTeacher && (
        <UserForm
          initial={editing}
          onSave={handleSave}
          onCancel={() => {
            setShowForm(false);
            setEditing(null);
          }}
        />
      )}

      <UserList
        users={users}
        onEdit={handleEdit}
        onDelete={handleDelete}
        canEdit={isTeacher}
      />
    </div>
  );
}
