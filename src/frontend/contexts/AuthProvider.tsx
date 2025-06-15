import React, { createContext, useContext, useState, useEffect } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';

interface AuthContextType {
  isAuthenticated: boolean;
  user: string | null;
  login: (email: string, password: string) => Promise<{ success: boolean; message?: string }>;
  logout: () => Promise<void>;
  isLoading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:8080';
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const checkAuthStatus = async () => {
    try {
      const token = await AsyncStorage.getItem('@auth_token');
      const userData = await AsyncStorage.getItem('@user_data');
      
      if (token && userData) {
        setIsAuthenticated(true);
        setUser(JSON.parse(userData).name || 'Usuário');
      }
    } catch (error) {
      console.error('Error checking auth status:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const login = async (email: string, password: string) => {
    
    try {
      setIsLoading(true);
      
      // Fazer login real com a API
      const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ email, password }),
      });

      if (response.ok) {
        const data = await response.json();
        
        // Assumindo que a resposta tem a estrutura: { token: string, user: { name: string, email: string } }
        const { idToken, user: userData } = data;

        console.log(`TOKEN_LOGIN: ${idToken}`);
        console.log(`DATA_LOGIN: ${userData}`);
        
        await AsyncStorage.setItem('@auth_token', idToken);
        await AsyncStorage.setItem('@user_data', JSON.stringify(userData));
        
        setIsAuthenticated(true);
        setUser(userData.name);
        
        return { success: true };
      } else {
        const errorData = await response.json().catch(() => ({}));
        return { 
          success: false, 
          message: errorData.message || 'Credenciais inválidas' 
        };
      }
      
    } catch (error) {
      console.error('Login error:', error);
      return { 
        success: false, 
        message: 'Erro ao conectar com o servidor' 
      };
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    try {
      await AsyncStorage.removeItem('@auth_token');
      await AsyncStorage.removeItem('@user_data');
      setIsAuthenticated(false);
      setUser(null);
    } catch (error) {
      console.error('Error during logout:', error);
    }
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, login, logout, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
