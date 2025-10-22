import React, { useState, useEffect } from 'react';

export default function UserForm({ initial, onSave, onCancel }) {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    dateOfBirth: '',
    designation: 'Student'
  });

  useEffect(() => {
    if (initial) {
      console.log('Form initial data:', initial); // Debug
      setFormData({
        name: initial.name || '',
        email: initial.email || '',
        dateOfBirth: initial.dateOfBirth?.split('T')[0] || '',
        designation: initial.designation || 'Student'
      });
    }
  }, [initial]);

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log('Form submitting:', formData); // Debug
    onSave(formData);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  return (
    <div style={{
      backgroundColor: '#f8f9fa',
      padding: '20px',
      borderRadius: '8px',
      marginBottom: '20px',
      border: '1px solid #dee2e6'
    }}>
      <h4>{initial ? '✏️ Edit User' : '➕ Add New User'}</h4>
      <form onSubmit={handleSubmit}>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '15px', marginTop: '15px' }}>
          <div>
            <label style={labelStyle}>Name *</label>
            <input
              type="text"
              name="name"
              value={formData.name}
              onChange={handleChange}
              required
              placeholder="Enter full name"
              style={inputStyle}
            />
          </div>

          <div>
            <label style={labelStyle}>Email *</label>
            <input
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
              placeholder="user@example.com"
              style={inputStyle}
            />
          </div>

          <div>
            <label style={labelStyle}>Date of Birth *</label>
            <input
              type="date"
              name="dateOfBirth"
              value={formData.dateOfBirth}
              onChange={handleChange}
              required
              style={inputStyle}
            />
          </div>

          <div>
            <label style={labelStyle}>Designation *</label>
            <select
              name="designation"
              value={formData.designation}
              onChange={handleChange}
              required
              style={inputStyle}
            >
              <option value="Student">Student</option>
              <option value="Teacher">Teacher</option>
            </select>
          </div>
        </div>

        <div style={{ marginTop: '15px', display: 'flex', gap: '10px' }}>
          <button type="submit" style={{...btnStyle, backgroundColor: '#007bff'}}>
            {initial ? '💾 Update User' : '✅ Create User'}
          </button>
          <button type="button" onClick={onCancel} style={{...btnStyle, backgroundColor: '#6c757d'}}>
            ❌ Cancel
          </button>
        </div>
      </form>
    </div>
  );
}

const labelStyle = {
  display: 'block',
  fontWeight: 'bold',
  marginBottom: '5px',
  fontSize: '14px'
};

const inputStyle = {
  width: '100%',
  padding: '10px',
  border: '1px solid #ced4da',
  borderRadius: '4px',
  fontSize: '14px'
};

const btnStyle = {
  padding: '10px 20px',
  border: 'none',
  borderRadius: '5px',
  color: 'white',
  cursor: 'pointer',
  fontWeight: 'bold',
  fontSize: '14px'
};
