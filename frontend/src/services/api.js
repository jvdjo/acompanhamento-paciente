import axios from 'axios';

const API_URL = 'http://localhost:5008/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle 401 responses
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('userName');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth
export const login = async (email, password) => {
  const response = await api.post('/auth/login', { email, password });
  return response.data;
};

// Pacientes
export const getPacientes = async () => {
  const response = await api.get('/pacientes');
  return response.data;
};

export const getPaciente = async (id) => {
  const response = await api.get(`/pacientes/${id}`);
  return response.data;
};

export const createPaciente = async (nome) => {
  const response = await api.post('/pacientes', { nome });
  return response.data;
};

export const deletePaciente = async (id) => {
  await api.delete(`/pacientes/${id}`);
};

// Sessões
export const getSessoes = async (pacienteId) => {
  const response = await api.get(`/pacientes/${pacienteId}/sessoes`);
  return response.data;
};

export const getSessao = async (pacienteId, sessaoId) => {
  const response = await api.get(`/pacientes/${pacienteId}/sessoes/${sessaoId}`);
  return response.data;
};

export const createSessao = async (pacienteId, data = null) => {
  const response = await api.post(`/pacientes/${pacienteId}/sessoes`, { data });
  return response.data;
};

export const updateSessao = async (pacienteId, sessaoId, anotacoes, notasTexto) => {
  await api.put(`/pacientes/${pacienteId}/sessoes/${sessaoId}`, { anotacoes, notasTexto });
};

export const deleteSessao = async (pacienteId, sessaoId) => {
  await api.delete(`/pacientes/${pacienteId}/sessoes/${sessaoId}`);
};

export default api;
