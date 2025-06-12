import React, { createContext, useContext, useState, useEffect } from "react";
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

    useEffect(() => {
        fetch(`${BASE_URL}/api/SomaConforme/por-categoria`)
            .then(async (res) => {
                if (!res.ok) {
                    // Tenta extrair JSON de erro, se possível
                    let errorJson = null;
                    try {
                        errorJson = await res.json();
                    } catch {}
                    throw new Error(errorJson?.error || `Erro ${res.status}`);
                }
                const text = await res.text();
                if (!text) return;
                const data = JSON.parse(text);
                console.log('Dados recebidos do backend:', data);
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
            })
            .catch((err) => {
                console.error('Erro ao buscar dados do backend:', err.message || err);
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
            });
    }, []);

    return (
        <CarteirasContext.Provider value={{ carteiras, carteirasPorPerfil }}>
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
