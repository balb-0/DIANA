import { View } from "react-native";
import StatusConformidade from "@/components/quantidadeConformidade";
import { useEffect, useState } from "react";

export default function Index() {
    // ✅ Mock direto no useState
    const [carteiras, setCarteiras] = useState({
        regulares: { quantidade: 12, total: 20 },
        inconformes: { quantidade: 8, total: 20 }
    });

    // ✅ Mocka a "chamada da API" só pra simular tempo de resposta
    useEffect(() => {
        setTimeout(() => {
            setCarteiras({
                regulares: { quantidade: 15, total: 20 },
                inconformes: { quantidade: 5, total: 20 }
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
        <View className="flex-row justify-around mt-4">
            <StatusConformidade {...statusRegulares} />
            <StatusConformidade {...statusInconformes} />
        </View>
    );
}
