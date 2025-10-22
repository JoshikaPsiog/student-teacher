import React, { useState, useEffect } from 'react';

export default function UserForm({ initial, onSave, onCancel }) {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    dob: '',
    role: 'Student',
    gender: 'Male',
    address: '',
    phonenumber: '',
    stage: '',
    passwordhash: ''
  });

  useEffect(() => {
    if (initial) {
      setFormData({
        name: initial.name || '',
        email: initial.email || '',
        dob: initial.dob?.split('T')[0] || '',
        role: initial.role || 'Student',
        gender: initial.gender || 'Male',
        address: initial.address || '',
        phonenumber: initial.phonenumber || '',
        stage: initial.stage || '',
        passwordhash: ''
      });
    }
  }, [initial]);

  const handleSubmit = (e) => {
    e.preventDefault();
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
              name="dob"
              value={formData.dob}
              onChange={handleChange}
              required
              style={inputStyle}
            />
          </div>

          <div>
            <label style={labelStyle}>Gender *</label>
            <select
              name="gender"
              value={formData.gender}
              onChange={handleChange}
              required
              style={inputStyle}
            >
              <option value="Male">Male</option>
              <option value="Female">Female</option>
              <option value="Other">Other</option>
            </select>
          </div>

          <div>
            <label style={labelStyle}>Role *</label>
            <select
              name="role"
              value={formData.role}
              onChange={handleChange}
              required
              style={inputStyle}
            >
              <option value="Student">Student</option>
              <option value="Teacher">Teacher</option>
            </select>
          </div>

          <div>
            <label style={labelStyle}>Phone Number</label>
            <input
              type="tel"
              name="phonenumber"
              value={formData.phonenumber}
              onChange={handleChange}
              placeholder="1234567890"
              style={inputStyle}
            />
          </div>

          <div>
            <label style={labelStyle}>Stage</label>
            <input
              type="text"
              name="stage"
              value={formData.stage}
              onChange={handleChange}
              placeholder="e.g., Senior"
              style={inputStyle}
            />
          </div>

          <div style={{ gridColumn: '1 / -1' }}>
            <label style={labelStyle}>Address</label>
            <input
              type="text"
              name="address"
              value={formData.address}
              onChange={handleChange}
              placeholder="Enter address"
              style={inputStyle}
            />
          </div>

          {!initial && (
            <div style={{ gridColumn: '1 / -1' }}>
              <label style={labelStyle}>Password *</label>
              <input
                type="password"
                name="passwordhash"
                value={formData.passwordhash}
                onChange={handleChange}
                required={!initial}
                placeholder="Enter password"
                style={inputStyle}
              />
            </div>
          )}
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
