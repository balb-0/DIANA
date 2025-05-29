import { View } from "react-native";
import { useEffect, useState } from "react";
import Welcome from "@/components/welcome"
import StatusConformidade from "@/components/quantidadeConformidade";
import CardCarteirasPerfil from "@/components/cardCarteirasPorPerfil";

export default function Index() {
    const [carteiras, setCarteiras] = useState({
        regulares: { quantidade: 0, total: 0 },
        inconformes: { quantidade: 0, total: 0 }
    });

    const [carteirasPorPerfil, setCarteirasPorPerfil] = useState({
        Moderado: { quantidade: 0, total: 0 },
        Conservador: { quantidade: 0, total: 0 },
        Sofisticado: { quantidade: 0, total: 0 },
        Todas: { quantidade: 0, total: 0 },
    });

    useEffect(() => {
        setTimeout(() => {
            setCarteiras({
                regulares: { quantidade: 124, total: 198 },
                inconformes: { quantidade: 74, total: 198 }
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
        <View className="h-full w-full justify-between bg-azul-claro-secundario">
            <Welcome
                user="João"
            />
            <View className="items-center flex-1 px-6">
                <View className="flex-row justify-center gap-8 pb-9 h-fit`">
                    <StatusConformidade {...statusRegulares} />
                    <StatusConformidade {...statusInconformes} />
                </View>
                <View className="flex-row justify-center gap-8 h-fit">
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
                <View className="flex-row justify-center gap-8 mt-8">
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
            </View>
        </View>
    );
}
