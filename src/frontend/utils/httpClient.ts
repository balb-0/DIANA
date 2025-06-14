import { authService } from '@/services/authService';

const API_BASE_URL = 'http://localhost:8080/api';

export const httpClient = {
  async get(endpoint: string) {
    return authService.makeAuthenticatedRequest(`${API_BASE_URL}${endpoint}`, {
      method: 'GET',
    });
  },

  async post(endpoint: string, data: any) {
    return authService.makeAuthenticatedRequest(`${API_BASE_URL}${endpoint}`, {
      method: 'POST',
      body: JSON.stringify(data),
    });
  },

  async put(endpoint: string, data: any) {
    return authService.makeAuthenticatedRequest(`${API_BASE_URL}${endpoint}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    });
  },

  async delete(endpoint: string) {
    return authService.makeAuthenticatedRequest(`${API_BASE_URL}${endpoint}`, {
      method: 'DELETE',
    });
  },
};
