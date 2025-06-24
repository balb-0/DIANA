import { Investment, ApiResponse } from '../types/investment';
import { authService } from './authService';

const API_BASE_URL = 'http://localhost:8080/api';

export class InvestmentService {
  private static async makeRequest<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    try {
      const response = await authService.makeAuthenticatedRequest(
        `${API_BASE_URL}${endpoint}`,
        options
      );

      if (!response.ok) {
        if (response.status === 404) {
          return {
            success: false,
            error: 'Cliente não encontrado ou sem investimentos',
            statusCode: 404
          };
        }
        
        if (response.status === 401) {
          return {
            success: false,
            error: 'Token de autenticação inválido ou expirado',
            statusCode: 401
          };
        }

        if (response.status === 500) {
          return {
            success: false,
            error: 'Erro interno do servidor',
            statusCode: 500
          };
        }

        return {
          success: false,
          error: `Erro na requisição: ${response.status}`,
          statusCode: response.status
        };
      }

      const data = await response.json();
      
      return {
        success: true,
        data,
        statusCode: response.status
      };

    } catch (error) {
      console.error('Erro na requisição:', error);
      return {
        success: false,
        error: 'Erro de conexão com o servidor',
        statusCode: 0
      };
    }
  }

  static async getInvestmentsByClientId(clientId: string): Promise<ApiResponse<Investment[]>> {
    if (!clientId || clientId.trim() === '') {
      return {
        success: false,
        error: 'ID do cliente é obrigatório',
        statusCode: 400
      };
    }

    return this.makeRequest<Investment[]>(`/investment/${clientId}`);
  }

  static isAuthenticated(): boolean {
    const token = authService.getStoredToken();
    const user = authService.getStoredUser();
    return !!(token && user);
  }

  static getCurrentUser() {
    return authService.getStoredUser();
  }
}