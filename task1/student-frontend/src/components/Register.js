import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register as registerAPI } from '../services/auth';
import '../styles/auth.css';

export default function Register() {
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    dob: '',
    role: 'Student',
    gender: 'Male',
    address: '',
    phonenumber: '',
    stage: ''
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      await registerAPI(formData);
      alert('✅ Registration successful! Please login.');
      navigate('/login');
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <div className="auth-header">
          <h2>✨ Create Account</h2>
        </div>
        
        <div className="auth-body">
          {error && (
            <div className="alert alert-danger">
              ⚠️ {error}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label className="form-label">👤 Full Name</label>
              <input
                type="text"
                className="form-input"
                name="name"
                value={formData.name}
                onChange={handleChange}
                placeholder="Enter your full name"
                required
              />
            </div>

            <div className="form-group">
              <label className="form-label">📧 Email Address</label>
              <input
                type="email"
                className="form-input"
                name="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="example@email.com"
                required
              />
            </div>

            <div className="form-group">
              <label className="form-label">🔑 Password</label>
              <input
                type="password"
                className="form-input"
                name="password"
                value={formData.password}
                onChange={handleChange}
                placeholder="Create a strong password"
                required
                minLength="6"
              />
            </div>

            <div className="form-group">
              <label className="form-label">📅 Date of Birth</label>
              <input
                type="date"
                className="form-input"
                name="dob"
                value={formData.dob}
                onChange={handleChange}
                required
              />
            </div>

            <div className="form-group">
              <label className="form-label">⚥ Gender</label>
              <select
                className="form-select"
                name="gender"
                value={formData.gender}
                onChange={handleChange}
                required
              >
                <option value="Male">Male</option>
                <option value="Female">Female</option>
                <option value="Other">Other</option>
              </select>
            </div>

            <div className="form-group">
              <label className="form-label">💼 Role</label>
              <select
                className="form-select"
                name="role"
                value={formData.role}
                onChange={handleChange}
                required
              >
                <option value="Student">Student</option>
                <option value="Teacher">Teacher</option>
              </select>
              <div className="form-help">
                <small>
                  <strong>Student:</strong> View-only | <strong>Teacher:</strong> Full CRUD access
                </small>
              </div>
            </div>

            <div className="form-group">
              <label className="form-label">📍 Address</label>
              <input
                type="text"
                className="form-input"
                name="address"
                value={formData.address}
                onChange={handleChange}
                placeholder="Your address"
              />
            </div>

            <div className="form-group">
              <label className="form-label">📱 Phone Number</label>
              <input
                type="tel"
                className="form-input"
                name="phonenumber"
                value={formData.phonenumber}
                onChange={handleChange}
                placeholder="1234567890"
              />
            </div>

            <div className="form-group">
              <label className="form-label">🎓 Stage</label>
              <input
                type="text"
                className="form-input"
                name="stage"
                value={formData.stage}
                onChange={handleChange}
                placeholder="e.g., Senior, Junior"
              />
            </div>

            <button 
              type="submit" 
              className="btn-primary"
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="spinner"></span>
                  Creating Account...
                </>
              ) : (
                'Register'
              )}
            </button>
          </form>
        </div>
        
        <div className="auth-footer">
          Already have an account? <a href="/login">Login here</a>
        </div>
      </div>
    </div>
  );
}
