const API_BASE_URL = 'http://localhost:8080/api';  // Trocar IP se rodar em celular

export interface Distribuicao {
  rendaFixa: number;
  rendaVariavel: number;
}

export interface PerfilClienteResponse {
  perfil: "Conservador" | "Moderado" | "Sofisticado";
  distribuicao: Distribuicao;
  valorTotal: number;
  dataCalculo: string;
  nomeCliente: string;
  clienteId: string;
}

export const perfilService = {
  async getPerfil(clienteId: string): Promise<PerfilClienteResponse | null> {
    try {
      const response = await fetch(`${API_BASE_URL}/Perfil/${clienteId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (!response.ok) {
        console.error(`Erro ao buscar perfil (${response.status}):`, await response.text());
        return null;
      }

      const data: PerfilClienteResponse = await response.json();
      return data;

    } catch (error) {
      console.error('Erro de conexão com o servidor:', error);
      return null;
    }
  },
};
