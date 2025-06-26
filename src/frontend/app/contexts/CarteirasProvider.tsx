import React, { createContext, useContext, useState, useEffect } from "react";
import AsyncStorage from '@react-native-async-storage/async-storage';
import { BASE_URL } from "../../constants";

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

    // Configuração da URL base da API
    const API_BASE_URL = BASE_URL || process.env.REACT_APP_API_URL || 'http://localhost:8080';

    // Função para obter o token do AsyncStorage
    const getAuthToken = async (): Promise<string | null> => {
        try {
            const token = await AsyncStorage.getItem('@auth_token');
            console.log(`TOKEN: ${token}`);
            return token;
        } catch (error) {
            console.error('Erro ao recuperar o token:', error);
            return null;
        }
    };

    // Função para testar conectividade com o backend
    const testBackendConnection = async (): Promise<boolean> => {
        const token = await getAuthToken();
        try {
            const response = await fetch(`${API_BASE_URL}/api/Compliance/clients-count?nonconforming=false`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                }
            });
            console.log('Teste de conectividade - Status:', response.status);
            return response.status !== 0;
        } catch (error) {
            console.error('Teste de conectividade falhou:', error);
            return false;
        }
    };

    // Função para buscar dados do SomaConforme (integração da feature branch)
    const fetchSomaConformeData = async (): Promise<void> => {
        const token = await getAuthToken();
        
        try {
            const response = await fetch(`${API_BASE_URL}/api/SomaConforme/por-categoria`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                }
            });

            if (!response.ok) {
                let errorJson = null;
                try {
                    errorJson = await response.json();
                } catch {}
                throw new Error(errorJson?.error || `Erro ${response.status}`);
            }

            const text = await response.text();
            if (!text) return;
            
            const data = JSON.parse(text);
            console.log('Dados recebidos do SomaConforme:', data);
            
            const novo: CarteirasPorPerfil = {
                Moderado: { quantidade: 0, total: 0 },
                Conservador: { quantidade: 0, total: 0 },
                Sofisticado: { quantidade: 0, total: 0 },
                Todas: { quantidade: 0, total: 0 },
            };
            
            let somaTodas = 0;
            data.forEach((item: any) => {
                if (item.perfilInvestidor === "Moderado") {
                    novo.Moderado.quantidade = item.totalConformes;
                    novo.Moderado.total = item.totalConformes;
                    somaTodas += item.totalConformes;
                } else if (item.perfilInvestidor === "Conservador") {
                    novo.Conservador.quantidade = item.totalConformes;
                    novo.Conservador.total = item.totalConformes;
                    somaTodas += item.totalConformes;
                } else if (item.perfilInvestidor === "Sofisticado") {
                    novo.Sofisticado.quantidade = item.totalConformes;
                    novo.Sofisticado.total = item.totalConformes;
                    somaTodas += item.totalConformes;
                }
            });
            
            novo.Todas.quantidade = somaTodas;
            novo.Todas.total = somaTodas;
            console.log('Objeto mapeado para o frontend:', novo);
            
            setCarteirasPorPerfil(novo);
            setCarteiras({
                Regulares: { quantidade: somaTodas, total: somaTodas },
                Inconformes: { quantidade: 0, total: somaTodas },
            });
            
        } catch (err) {
            console.error('Erro ao buscar dados do SomaConforme:', err);
            throw err;
        }
    };

    // Função para buscar dados de clientes conformes (fallback)
    const fetchConformingClients = async (): Promise<number> => {
        const token = await getAuthToken();

        if (!token) {
            throw new Error('Token de autenticação não encontrado');
        }

        const url = `${API_BASE_URL}/api/Compliance/clients-count?nonconforming=false`;
        console.log('Fazendo requisição para:', url);

        try {
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                },
            });

            if (!response.ok) {
                if (response.status === 401) {
                    throw new Error('Token inválido ou expirado');
                }
                if (response.status === 404) {
                    throw new Error(`Endpoint não encontrado: ${url}`);
                }
                throw new Error(`Erro ao buscar clientes conformes: ${response.status} - ${response.statusText}`);
            }

            const data = await response.json();
            console.log('Dados recebidos (conformes):', data);
            return data;

        } catch (fetchError) {
            console.error('Erro detalhado na requisição:', fetchError);
            if (fetchError instanceof TypeError && fetchError.message.includes('Network request failed')) {
                throw new Error(`Falha de rede ao conectar com ${API_BASE_URL}. Verifique se o backend está rodando.`);
            }
            throw fetchError;
        }
    };

    // Função para buscar dados de clientes não conformes (fallback)
    const fetchNonConformingClients = async (): Promise<number> => {
        const token = await getAuthToken();
        
        if (!token) {
            throw new Error('Token de autenticação não encontrado');
        }

        const url = `${API_BASE_URL}/api/Compliance/clients-count?nonconforming=true`;
        console.log('Fazendo requisição para:', url);

        try {
            const response = await fetch(url, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`,
                },
            });

            if (!response.ok) {
                if (response.status === 401) {
                    throw new Error('Token inválido ou expirado');
                }
                if (response.status === 404) {
                    throw new Error(`Endpoint não encontrado: ${url}`);
                }
                throw new Error(`Erro ao buscar clientes não conformes: ${response.status} - ${response.statusText}`);
            }

            const data = await response.json();
            console.log('Dados recebidos (não conformes):', data);
            return data;

        } catch (fetchError) {
            console.error('Erro detalhado na requisição (não conformes):', fetchError);
            if (fetchError instanceof TypeError && fetchError.message.includes('Network request failed')) {
                throw new Error(`Falha de rede ao conectar com ${API_BASE_URL}. Verifique se o backend está rodando.`);
            }
            throw fetchError;
        }
    };

    // Função para atualizar dados das carteiras
    const refreshData = async (): Promise<void> => {
        setIsLoading(true);
        setError(null);

        try {
            // Primeiro, teste a conectividade
            console.log('Testando conectividade com backend...');
            const isConnected = await testBackendConnection();
            
            if (!isConnected) {
                throw new Error('Não foi possível conectar ao backend. Verifique se o servidor está rodando na porta 8080.');
            }

            console.log('Conectividade OK. Buscando dados...');

            // Tenta primeiro buscar dados do SomaConforme (nova integração)
            try {
                await fetchSomaConformeData();
                console.log('Dados carregados via SomaConforme');
            } catch (somaConformeError) {
                console.warn('Falha ao carregar dados via SomaConforme, usando fallback:', somaConformeError);
                
                // Fallback para endpoints de Compliance
                const [conformingCount, nonConformingCount] = await Promise.all([
                    fetchConformingClients(),
                    fetchNonConformingClients()
                ]);

                const totalClients = conformingCount + nonConformingCount;

                console.log('Dados processados (fallback):', {
                    conformes: conformingCount,
                    naoConformes: nonConformingCount,
                    total: totalClients
                });

                // Atualiza estado das carteiras
                setCarteiras({
                    Regulares: { quantidade: conformingCount, total: totalClients },
                    Inconformes: { quantidade: nonConformingCount, total: totalClients },
                });

                // Dados por perfil como mock
                setCarteirasPorPerfil({
                    Conservador: { quantidade: Math.floor(conformingCount * 0.15), total: Math.floor(totalClients * 0.15) },
                    Moderado: { quantidade: Math.floor(conformingCount * 0.56), total: Math.floor(totalClients * 0.56) },
                    Sofisticado: { quantidade: Math.floor(conformingCount * 0.16), total: Math.floor(totalClients * 0.29) },
                    Todas: { quantidade: conformingCount, total: totalClients },
                });
            }

        } catch (err) {
            let errorMessage = 'Erro desconhecido';
            
            if (err instanceof Error) {
                if (err.message.includes('Network request failed')) {
                    errorMessage = 'Falha na conexão com o servidor. Verifique se o backend está rodando.';
                } else if (err.message.includes('fetch')) {
                    errorMessage = 'Erro de rede. Verifique sua conexão e configuração de CORS.';
                } else {
                    errorMessage = err.message;
                }
            }
            
            setError(errorMessage);
            console.error('Erro ao buscar dados das carteiras:', err);
            
            // Estado de fallback em caso de erro
            setCarteirasPorPerfil({
                Conservador: { quantidade: 0, total: 0 },
                Moderado: { quantidade: 0, total: 0 },
                Sofisticado: { quantidade: 0, total: 0 },
                Todas: { quantidade: 0, total: 0 },
            });
            setCarteiras({
                Regulares: { quantidade: 0, total: 0 },
                Inconformes: { quantidade: 0, total: 0 },
            });
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