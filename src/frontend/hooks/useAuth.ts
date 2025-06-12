import { useState, useEffect } from 'react';
import { authService, LoginRequest, UserInfo } from '@/services/authService';

export const useAuth = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [user, setUser] = useState<UserInfo | null>(null);
  const [error, setError] = useState<string | null>(null);

  // Check for stored user on hook initialization
  useEffect(() => {
    const storedUser = authService.getStoredUser();
    if (storedUser) {
      setUser(storedUser);
    }
  }, []);

  const login = async (credentials: LoginRequest) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await authService.login(credentials);
      
      if (response.success && response.user) {
        setUser(response.user);
        return { success: true };
      } else {
        setError(response.message || 'Erro ao fazer login');
        return { success: false, message: response.message };
      }
    } catch (error) {
      const errorMessage = 'Erro inesperado ao fazer login';
      setError(errorMessage);
      return { success: false, message: errorMessage };
    } finally {
      setIsLoading(false);
    }
  };

  const logout = async () => {
    await authService.logout();
    setUser(null);
    setError(null);
  };

  const getToken = () => {
    return authService.getStoredToken();
  };

  return {
    user,
    isLoading,
    error,
    login,
    logout,
    getToken,
    isAuthenticated: !!user,
  };
};
