import React, { createContext, useContext, useState, useEffect } from "react";
import AsyncStorage from '@react-native-async-storage/async-storage';
import { statisticsService, EstatisticasCompletas } from "@/services/statisticsService";

type Carteiras = {
    Regulares: { quantidade: number; total: number };
    Inconformes: { quantidade: number; total: number };
};

type CarteirasPorPerfil = {
    Moderado: { quantidade: number; total: number };
    Conservador: { quantidade: number; total: number };
    Sofisticado: { quantidade: number; total: number };
    Todas: { quantidade: number; total: number };
};

type ContextType = {
    carteiras: Carteiras;
    carteirasPorPerfil: CarteirasPorPerfil;
    isLoading: boolean;
    error: string | null;
    refreshData: () => Promise<void>;
};

const CarteirasContext = createContext<ContextType | null>(null);

export const CarteirasProvider = ({ children }: { children: React.ReactNode }) => {
    const [carteiras, setCarteiras] = useState<Carteiras>({
        Regulares: { quantidade: 0, total: 0 },
        Inconformes: { quantidade: 0, total: 0 },
    });

    const [carteirasPorPerfil, setCarteirasPorPerfil] = useState<CarteirasPorPerfil>({
        Moderado: { quantidade: 0, total: 0 },
        Conservador: { quantidade: 0, total: 0 },
        Sofisticado: { quantidade: 0, total: 0 },
        Todas: { quantidade: 0, total: 0 },
    });

    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // Função para atualizar dados das carteiras usando o novo serviço
    const refreshData = async (): Promise<void> => {
        setIsLoading(true);
        setError(null);

        try {
            console.log('Iniciando busca de estatísticas...');

            // Usa o novo serviço de estatísticas
            const estatisticas: EstatisticasCompletas = await statisticsService.getEstatisticasCompletas();

            console.log('Estatísticas recebidas:', estatisticas);

            // Atualiza estado das carteiras com dados reais
            setCarteiras({
                Regulares: {
                    quantidade: estatisticas.todas.conformes,
                    total: estatisticas.todas.total
                },
                Inconformes: {
                    quantidade: estatisticas.todas.inconformes,
                    total: estatisticas.todas.total
                },
            });

            // Atualiza dados por perfil com dados reais
            setCarteirasPorPerfil({
                Conservador: {
                    quantidade: estatisticas.conservador.conformes,
                    total: estatisticas.conservador.total
                },
                Moderado: {
                    quantidade: estatisticas.moderado.conformes,
                    total: estatisticas.moderado.total
                },
                Sofisticado: {
                    quantidade: estatisticas.sofisticado.conformes,
                    total: estatisticas.sofisticado.total
                },
                Todas: {
                    quantidade: estatisticas.todas.conformes,
                    total: estatisticas.todas.total
                },
            });

            console.log('Estados atualizados com sucesso');

        } catch (err) {
            let errorMessage = 'Erro ao carregar estatísticas';

            if (err instanceof Error) {
                errorMessage = err.message;
            }

            setError(errorMessage);
            console.error('Erro ao buscar estatísticas:', err);
        } finally {
            setIsLoading(false);
        }
    };

    // Carrega dados na inicialização
    useEffect(() => {
        refreshData();
    }, []);

    return (
        <CarteirasContext.Provider
            value={{
                carteiras,
                carteirasPorPerfil,
                isLoading,
                error,
                refreshData
            }}
        >
            {children}
        </CarteirasContext.Provider>
    );
};

export const useCarteiras = () => {
    const context = useContext(CarteirasContext);
    if (!context) {
        throw new Error("useCarteiras must be used within a CarteirasProvider");
    }
    return context;
};