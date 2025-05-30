import React, { createContext, useContext, useState, useEffect } from "react";

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
        setTimeout(() => {
            setCarteiras({
                Regulares: { quantidade: 124, total: 198 },
                Inconformes: { quantidade: 74, total: 198 },
            });
        }, 1000);

        setTimeout(() => {
            setCarteirasPorPerfil({
                Conservador: { quantidade: 19, total: 30 },
                Moderado: { quantidade: 69, total: 110 },
                Sofisticado: { quantidade: 20, total: 58 },
                Todas: { quantidade: 124, total: 198 },
            });
        }, 1000);
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
