import { authService } from './authService';

export interface ClienteCarteira {
  clienteId: string;
  clienteNome: string;
}

export async function getClientesPorRisco(tipoRisco: string): Promise<ClienteCarteira[]> {
  try {
    const response = await authService.makeAuthenticatedRequest(
      `http://localhost:8080/api/Carteiras/clientes/risco-declarado/${tipoRisco}`,
      {
        method: 'GET',
      }
    );

    if (!response.ok) {
      console.error('Erro na resposta da API:', response.status);
      return [];
    }

    const data = await response.json();

    if (data && data.success) {
      return data.data;
    } else {
      console.error('Resposta inválida da API:', data);
      return [];
    }
  } catch (error) {
    console.error('Erro ao buscar clientes por risco:', error);
    return [];
  }
}
