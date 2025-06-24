import { useState, useEffect, useCallback } from 'react';
import { Investment, InvestmentSummary } from '../types/investment';
import { InvestmentService } from '../services/investmentService';

interface UseInvestmentsReturn {
  investments: Investment[];
  summary: InvestmentSummary | null;
  loading: boolean;
  error: string | null;
  refetch: () => Promise<void>;
  clearError: () => void;
  isAuthenticated: boolean;
}

export const useInvestments = (clientId: string): UseInvestmentsReturn => {
  const [investments, setInvestments] = useState<Investment[]>([]);
  const [summary, setSummary] = useState<InvestmentSummary | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);

  const calculateSummary = useCallback((investmentsList: Investment[]): InvestmentSummary => {
    const totalInvestments = investmentsList.length;
    
    // Usar currentValue se valorTotal não existir
    const totalValue = investmentsList.reduce((sum, inv) => {
      return sum + (inv.valorTotal || inv.currentValue || 0);
    }, 0);
    
    const investmentsByType: { [key: string]: number } = {};
    const investmentsBySubgroup: { [key: string]: number } = {};

    investmentsList.forEach(investment => {
      const investmentValue = investment.valorTotal || investment.currentValue || 0;
      
      if (investment.ativo) {
        const tipo = investment.ativo.tipo;
        investmentsByType[tipo] = (investmentsByType[tipo] || 0) + investmentValue;

        const subgrupo = investment.ativo.subgrupo;
        investmentsBySubgroup[subgrupo] = (investmentsBySubgroup[subgrupo] || 0) + investmentValue;
      } else {
        // Se não tem ativo, usar productType como fallback
        const tipo = investment.productType || 'Outros';
        investmentsByType[tipo] = (investmentsByType[tipo] || 0) + investmentValue;
        investmentsBySubgroup['Outros'] = (investmentsBySubgroup['Outros'] || 0) + investmentValue;
      }
    });

    return {
      totalInvestments,
      totalValue,
      investmentsByType,
      investmentsBySubgroup
    };
  }, []);

  const fetchInvestments = useCallback(async () => {
    const authenticated = InvestmentService.isAuthenticated();
    setIsAuthenticated(authenticated);

    if (!authenticated) {
      setError('Usuário não autenticado. Faça login novamente.');
      setLoading(false);
      return;
    }

    if (!clientId) {
      setError('ID do cliente não fornecido');
      setLoading(false);
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const response = await InvestmentService.getInvestmentsByClientId(clientId);

      if (response.success && response.data) {
        setInvestments(response.data);
        setSummary(calculateSummary(response.data));
      } else {
        if (response.statusCode === 401) {
          setIsAuthenticated(false);
          setError('Sessão expirada. Faça login novamente.');
        } else {
          setError(response.error || 'Erro desconhecido');
        }
        setInvestments([]);
        setSummary(null);
      }
    } catch (err) {
      setError('Erro inesperado ao buscar investimentos');
      setInvestments([]);
      setSummary(null);
    } finally {
      setLoading(false);
    }
  }, [clientId, calculateSummary]);

  const refetch = useCallback(async () => {
    await fetchInvestments();
  }, [fetchInvestments]);

  const clearError = useCallback(() => {
    setError(null);
  }, []);

  useEffect(() => {
    fetchInvestments();
  }, [fetchInvestments]);

  return {
    investments,
    summary,
    loading,
    error,
    refetch,
    clearError,
    isAuthenticated
  };
};