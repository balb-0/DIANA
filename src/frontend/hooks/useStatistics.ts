import { useState, useEffect } from 'react';
import { statisticsService, EstatisticasCategoria } from '@/services/statisticsService';

export function useStatistics() {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const getEstatisticasCategoria = async (
        categoria: 'Conservador' | 'Moderado' | 'Sofisticado'
    ): Promise<EstatisticasCategoria> => {
        setIsLoading(true);
        setError(null);

        try {
            const stats = await statisticsService.getEstatisticasCategoria(categoria);
            return stats;
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Erro desconhecido';
            setError(errorMessage);
            console.error(`Erro ao buscar estatísticas de ${categoria}:`, err);
            return { total: 0, conformes: 0, inconformes: 0 };
        } finally {
            setIsLoading(false);
        }
    };

    const getEstatisticasCompletas = async () => {
        setIsLoading(true);
        setError(null);

        try {
            const stats = await statisticsService.getEstatisticasCompletas();
            return stats;
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Erro desconhecido';
            setError(errorMessage);
            console.error('Erro ao buscar estatísticas completas:', err);
            return {
                todas: { total: 0, conformes: 0, inconformes: 0 },
                conservador: { total: 0, conformes: 0, inconformes: 0 },
                moderado: { total: 0, conformes: 0, inconformes: 0 },
                sofisticado: { total: 0, conformes: 0, inconformes: 0 }
            };
        } finally {
            setIsLoading(false);
        }
    };

    return {
        isLoading,
        error,
        getEstatisticasCategoria,
        getEstatisticasCompletas
    };
}
