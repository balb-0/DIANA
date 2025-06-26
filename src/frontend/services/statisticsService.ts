import { authService } from './authService';
import { perfilService } from './perfilService';
import { clientService } from './clientService';

export interface ClienteCarteira {
    clienteId: string;
    clienteNome: string;
}

export interface ClienteComStatus {
    clienteId: string;
    clienteNome: string;
    isRegular: boolean;
    perfilRecomendado?: string;
    perfilDeclarado?: string;
}

export interface EstatisticasCategoria {
    total: number;
    conformes: number;
    inconformes: number;
}

export interface EstatisticasCompletas {
    todas: EstatisticasCategoria;
    conservador: EstatisticasCategoria;
    moderado: EstatisticasCategoria;
    sofisticado: EstatisticasCategoria;
}

class StatisticsService {
    private async getClientesPorRisco(tipoRisco: string): Promise<ClienteCarteira[]> {
        try {
            const response = await authService.makeAuthenticatedRequest(
                `http://localhost:8080/api/Carteiras/clientes/risco-declarado/${tipoRisco}`,
                {
                    method: 'GET',
                }
            );

            if (!response.ok) {
                console.error(`Erro ao buscar clientes ${tipoRisco}:`, response.status);
                return [];
            }

            const data = await response.json();

            if (data && data.success) {
                return data.data || [];
            } else {
                console.error(`Resposta inválida para ${tipoRisco}:`, data);
                return [];
            }
        } catch (error) {
            console.error(`Erro ao buscar clientes ${tipoRisco}:`, error);
            return [];
        }
    }

    private async verificarConformidadeCliente(clienteId: string): Promise<{
        isRegular: boolean;
        perfilRecomendado?: string;
        perfilDeclarado?: string;
    }> {
        try {
            // Busca o perfil recomendado e o perfil declarado em paralelo
            const [perfilRecomendado, perfilDeclarado] = await Promise.all([
                perfilService.getPerfil(clienteId),
                clientService.getPerfilDeRisco(clienteId)
            ]);

            if (!perfilRecomendado || !perfilDeclarado) {
                console.warn(`Dados incompletos para cliente ${clienteId}`);
                return { isRegular: false };
            }

            const isRegular = perfilDeclarado.perfil_de_risco === perfilRecomendado.perfil;

            return {
                isRegular,
                perfilRecomendado: perfilRecomendado.perfil,
                perfilDeclarado: perfilDeclarado.perfil_de_risco
            };
        } catch (error) {
            console.error(`Erro ao verificar conformidade do cliente ${clienteId}:`, error);
            return { isRegular: false };
        }
    }

    private async processarClientesCategoria(
        clientes: ClienteCarteira[]
    ): Promise<ClienteComStatus[]> {
        const clientesComStatus: ClienteComStatus[] = [];

        // Processa em lotes de 10 para não sobrecarregar a API
        const batchSize = 10;
        for (let i = 0; i < clientes.length; i += batchSize) {
            const batch = clientes.slice(i, i + batchSize);

            const batchPromises = batch.map(async (cliente) => {
                const status = await this.verificarConformidadeCliente(cliente.clienteId);
                return {
                    ...cliente,
                    ...status
                };
            });

            const batchResults = await Promise.all(batchPromises);
            clientesComStatus.push(...batchResults);

            // Pequena pausa entre lotes para não sobrecarregar a API
            if (i + batchSize < clientes.length) {
                await new Promise(resolve => setTimeout(resolve, 100));
            }
        }

        return clientesComStatus;
    }

    private calcularEstatisticas(clientes: ClienteComStatus[]): EstatisticasCategoria {
        const total = clientes.length;
        const conformes = clientes.filter(c => c.isRegular).length;
        const inconformes = total - conformes;

        return {
            total,
            conformes,
            inconformes
        };
    }

    async getEstatisticasCompletas(): Promise<EstatisticasCompletas> {
        try {
            console.log('Iniciando busca de estatísticas completas...');

            // Busca todos os clientes por categoria em paralelo
            const [
                clientesConservador,
                clientesModerado,
                clientesSofisticado
            ] = await Promise.all([
                this.getClientesPorRisco('Conservador'),
                this.getClientesPorRisco('Moderado'),
                this.getClientesPorRisco('Sofisticado')
            ]);

            console.log('Clientes encontrados:', {
                conservador: clientesConservador.length,
                moderado: clientesModerado.length,
                sofisticado: clientesSofisticado.length
            });

            // Processa conformidade para cada categoria
            const [
                conservadorComStatus,
                moderadoComStatus,
                sofisticadoComStatus
            ] = await Promise.all([
                this.processarClientesCategoria(clientesConservador),
                this.processarClientesCategoria(clientesModerado),
                this.processarClientesCategoria(clientesSofisticado)
            ]);

            // Combina todos os clientes
            const todosClientes = [
                ...conservadorComStatus,
                ...moderadoComStatus,
                ...sofisticadoComStatus
            ];

            // Calcula estatísticas para cada categoria
            const estatisticas: EstatisticasCompletas = {
                todas: this.calcularEstatisticas(todosClientes),
                conservador: this.calcularEstatisticas(conservadorComStatus),
                moderado: this.calcularEstatisticas(moderadoComStatus),
                sofisticado: this.calcularEstatisticas(sofisticadoComStatus)
            };

            console.log('Estatísticas calculadas:', estatisticas);
            return estatisticas;

        } catch (error) {
            console.error('Erro ao calcular estatísticas completas:', error);

            // Retorna estatísticas vazias em caso de erro
            return {
                todas: { total: 0, conformes: 0, inconformes: 0 },
                conservador: { total: 0, conformes: 0, inconformes: 0 },
                moderado: { total: 0, conformes: 0, inconformes: 0 },
                sofisticado: { total: 0, conformes: 0, inconformes: 0 }
            };
        }
    }

    // Método para obter estatísticas de uma categoria específica
    async getEstatisticasCategoria(categoria: 'Conservador' | 'Moderado' | 'Sofisticado'): Promise<EstatisticasCategoria> {
        try {
            const clientes = await this.getClientesPorRisco(categoria);
            const clientesComStatus = await this.processarClientesCategoria(clientes);
            return this.calcularEstatisticas(clientesComStatus);
        } catch (error) {
            console.error(`Erro ao calcular estatísticas para ${categoria}:`, error);
            return { total: 0, conformes: 0, inconformes: 0 };
        }
    }
}

export const statisticsService = new StatisticsService();
