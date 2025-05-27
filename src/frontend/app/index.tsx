import { View } from "react-native";
import StatusConformidade from "@/components/quantidadeConformidade";
import CardCarteirasPerfil from "@/components/cardCarteirasPorPerfil";
import { useEffect, useState } from "react";

export default function Index() {
    // ✅ Mock direto no useState
    const [carteiras, setCarteiras] = useState({
        regulares: { quantidade: 12, total: 20 },
        inconformes: { quantidade: 8, total: 20 }
    });

    const [carteirasPorPerfil, setCarteirasPorPerfil] = useState({
    Moderado: { quantidade: 0, total: 0 },
    Conservador: { quantidade: 0, total: 0 },
    Sofisticado: { quantidade: 0, total: 0 },
    Todas: { quantidade: 0, total: 0 },
    });

    // ✅ Mocka a "chamada da API" só pra simular tempo de resposta
    useEffect(() => {
        setTimeout(() => {
            setCarteiras({
                regulares: { quantidade: 15, total: 20 },
                inconformes: { quantidade: 5, total: 20 }
            });
        }, 1000);

        setTimeout(() => {
            setCarteirasPorPerfil({
            Moderado: { quantidade: 69, total: 110 },
            Conservador: { quantidade: 15, total: 110 },
            Sofisticado: { quantidade: 20, total: 110 },
            Todas: { quantidade: 104, total: 110 },
            });
        }, 1000);
    }, []);

    const statusRegulares = {
        corFundo: 'azul' as const,
        corQuadrado: 'azul' as const,
        tipo: 'Conformidade',
        quantidade: carteiras.regulares.quantidade,
        total: carteiras.regulares.total,
        onPress: () => console.log('Abriu regulares'),
    };

    const statusInconformes = {
        corFundo: 'vermelho' as const,
        corQuadrado: 'vermelho' as const,
        tipo: 'Irregulares',
        quantidade: carteiras.inconformes.quantidade,
        total: carteiras.inconformes.total,
        onPress: () => console.log('Abriu inconformes'),
    };

    return (
        <>
            <View className="flex-row justify-around mt-4">
                <StatusConformidade {...statusRegulares} />
                <StatusConformidade {...statusInconformes} />
            </View>
            
            <View className="flex-row justify-around mt-4">
                <CardCarteirasPerfil
                    perfil="Moderado"
                    quantidade={carteirasPorPerfil.Moderado.quantidade}
                    total={carteirasPorPerfil.Moderado.total}
                />
                <CardCarteirasPerfil
                    perfil="Sofisticado"
                    quantidade={carteirasPorPerfil.Sofisticado.quantidade}
                    total={carteirasPorPerfil.Sofisticado.total}
                />
            </View>

            <View className="flex-row justify-around mt-4">
                <CardCarteirasPerfil
                    perfil="Conservador"
                    quantidade={carteirasPorPerfil.Conservador.quantidade}
                    total={carteirasPorPerfil.Conservador.total}
                />
                <CardCarteirasPerfil
                    perfil="Todas"
                    quantidade={carteirasPorPerfil.Todas.quantidade}
                    total={carteirasPorPerfil.Todas.total}
                />
            </View>
        </>
    );
}