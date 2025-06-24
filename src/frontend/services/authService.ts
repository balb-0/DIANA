import AsyncStorage from '@react-native-async-storage/async-storage';

const API_BASE_URL = 'http://localhost:8080/api';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserInfo {
  uid: string;
  email: string;
  displayName?: string;
  emailVerified: boolean;
}

export interface AuthResponse {
  success: boolean;
  message?: string;
  idToken?: string;
  user?: UserInfo;
}

export const authService = {
  async login(credentials: LoginRequest): Promise<AuthResponse> {
    try {
      const response = await fetch(`${API_BASE_URL}/auth/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(credentials),
      });

      const data: AuthResponse = await response.json();

      if (data.success && data.idToken) {
        // Store JWT token for future requests using AsyncStorage
        await AsyncStorage.setItem('@auth_token', data.idToken);
        await AsyncStorage.setItem('@user', JSON.stringify(data.user));
      }

      return data;
    } catch (error) {
      console.error('Login error:', error);
      return {
        success: false,
        message: 'Erro de conexão com o servidor',
      };
    }
  },

  async logout(): Promise<void> {
    await AsyncStorage.removeItem('@auth_token');
    await AsyncStorage.removeItem('@user');
  },

  async getStoredToken(): Promise<string | null> {
    try {
      return await AsyncStorage.getItem('@auth_token');
    } catch (error) {
      console.error('Erro ao recuperar token:', error);
      return null;
    }
  },

  async getStoredUser(): Promise<UserInfo | null> {
    try {
      const user = await AsyncStorage.getItem('@user');
      return user ? JSON.parse(user) : null;
    } catch (error) {
      console.error('Erro ao recuperar usuário:', error);
      return null;
    }
  },

  // Helper method to make authenticated requests
  async makeAuthenticatedRequest(url: string, options: RequestInit = {}): Promise<Response> {
    const token = await this.getStoredToken();

    // Garante que headers seja sempre um objeto simples
    let headers: Record<string, string> = {
      'Content-Type': 'application/json',
    };

    if (options.headers) {
      if (options.headers instanceof Headers) {
        options.headers.forEach((value, key) => {
          headers[key] = value;
        });
      } else {
        headers = { ...headers, ...(options.headers as Record<string, string>) };
      }
    }

    if (token) {
      headers['Authorization'] = `Bearer ${token}`;
    }

    return fetch(url, {
      ...options,
      headers,
    });
  },
};
