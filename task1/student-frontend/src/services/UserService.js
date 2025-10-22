import axios from 'axios';

const API_URL = 'http://localhost:5164/api';

export const getUsers = async (token) => {
  const response = await axios.get(`${API_URL}/Teacher`, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

export const createUser = async (token, user) => {
  const response = await axios.post(`${API_URL}/Teacher`, user, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

export const updateUser = async (token, id, user) => {
  const response = await axios.put(`${API_URL}/Teacher/${id}`, user, {
    headers: { Authorization: `Bearer ${token}` }
  });
  return response.data;
};

export const deleteUser = async (token, id) => {
  await axios.delete(`${API_URL}/Teacher/${id}`, {
    headers: { Authorization: `Bearer ${token}` }
  });
};
