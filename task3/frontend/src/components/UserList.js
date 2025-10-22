import React from 'react';

export default function UserList({ users, onEdit, onDelete, canEdit }) {
  if (!users || users.length === 0) {
    return (
      <div style={{ padding: '20px', textAlign: 'center', color: '#666' }}>
        ⚠️ No users found. {canEdit && 'Click "Add New User" to create one.'}
      </div>
    );
  }

  return (
    <div style={{ marginTop: '20px' }}>
      <h4>Users List ({users.length})</h4>
      <table style={{ width: '100%', borderCollapse: 'collapse', marginTop: '15px' }}>
        <thead>
          <tr style={{ backgroundColor: '#667eea', color: 'white' }}>
            <th style={thStyle}>ID</th>
            <th style={thStyle}>Name</th>
            <th style={thStyle}>Email</th>
            <th style={thStyle}>Designation</th>
            <th style={thStyle}>Date of Birth</th>
            {canEdit && <th style={thStyle}>Actions</th>}
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id} style={trStyle}>  {/* ✅ Added key={user.id} */}
              <td style={tdStyle}>{user.id}</td>
              <td style={tdStyle}>{user.name}</td>
              <td style={tdStyle}>{user.email}</td>
              <td style={tdStyle}>
                <span style={{
                  padding: '4px 12px',
                  borderRadius: '12px',
                  backgroundColor: user.designation === 'Teacher' ? '#d4edda' : '#d1ecf1',
                  color: user.designation === 'Teacher' ? '#155724' : '#0c5460',
                  fontSize: '13px',
                  fontWeight: 'bold'
                }}>
                  {user.designation}
                </span>
              </td>
              <td style={tdStyle}>{new Date(user.dateOfBirth).toLocaleDateString()}</td>
              {canEdit && (
                <td style={tdStyle}>
                  <button 
                    onClick={() => onEdit(user)} 
                    style={{...btnStyle, backgroundColor: '#ffc107', marginRight: '8px'}}
                  >
                    ✏️ Edit
                  </button>
                  <button 
                    onClick={() => onDelete(user.id)} 
                    style={{...btnStyle, backgroundColor: '#dc3545'}}
                  >
                    🗑️ Delete
                  </button>
                </td>
              )}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

// Styles
const thStyle = { padding: '12px', textAlign: 'left', borderBottom: '2px solid #ddd' };
const tdStyle = { padding: '12px', borderBottom: '1px solid #eee' };
const trStyle = { ':hover': { backgroundColor: '#f5f5f5' } };
const btnStyle = {
  padding: '6px 12px',
  border: 'none',
  borderRadius: '4px',
  color: 'white',
  cursor: 'pointer',
  fontSize: '13px',
  fontWeight: 'bold'
};
