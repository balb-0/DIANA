const API_BASE_URL = "http://localhost:8080"; 

export interface PerfilDeRiscoResponse {
  id: string;
  perfil_de_risco: "Conservador" | "Moderado" | "Sofisticado" | "";
}

export const clientService = {
  async getPerfilDeRisco(clienteId: string): Promise<PerfilDeRiscoResponse | null> {
    try {
      const response = await fetch(`${API_BASE_URL}/Client/${clienteId}/perfil`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });

      if (!response.ok) {
        console.error(`Erro ${response.status}:`, await response.text());
        return null;
      }

      const data: PerfilDeRiscoResponse = await response.json();
      return data;
    } catch (error) {
      console.error("Erro na requisição ao perfil de risco:", error);
      return null;
    }
  },
};
