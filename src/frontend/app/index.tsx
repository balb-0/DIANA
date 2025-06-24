import BarraPesquisa from "@/components/barraPesquisa";
import CardCarteirasPerfil from "@/components/cardCarteirasPorPerfil";
import StatusConformidade from "@/components/quantidadeConformidade";
import { RelativePathString, useRouter } from "expo-router";
import { View, Text, ActivityIndicator } from "react-native";
import { useCarteiras } from "../contexts/CarteirasProvider";

export default function Index() {
    const router = useRouter();
    const { carteiras, carteirasPorPerfil, isLoading, error } = useCarteiras();

    // Debug logs
    console.log('[Index] Dados das carteiras:', {
        carteiras,
        carteirasPorPerfil,
        isLoading,
        error
    });

    const navegarParaCategoria = (nome: string) => {
        router.push(`/categoria/${nome}` as RelativePathString);
    };

    const statusRegulares = {
        corFundo: "azul" as const,
        corQuadrado: "azul" as const,
        tipo: "Conformidade",
        quantidade: carteiras.Regulares.quantidade,
        total: carteiras.Regulares.total,
        onPress: () => navegarParaCategoria("Regulares"),
    };

    const statusInconformes = {
        corFundo: "vermelho" as const,
        corQuadrado: "vermelho" as const,
        tipo: "Irregulares",
        quantidade: carteiras.Inconformes.quantidade,
        total: carteiras.Inconformes.total, onPress: () => navegarParaCategoria("Inconformes"),
    };

    // Mostrar indicador de carregamento
    if (isLoading) {
        return (
            <View className="flex-1 w-full justify-center items-center bg-azul-claro-secundario">
                <ActivityIndicator size="large" color="#0066CC" />
                <Text className="mt-4 text-gray-600">Carregando dados das carteiras...</Text>
            </View>
        );
    }

    // Mostrar erro se houver
    if (error) {
        return (
            <View className="flex-1 w-full justify-center items-center bg-azul-claro-secundario px-6">
                <Text className="text-red-500 text-center mb-4">
                    Erro ao carregar dados: {error}
                </Text>
                <Text className="text-gray-600 text-center">
                    Verifique se você está logado e se o backend está rodando.
                </Text>
            </View>
        );
    }

    return (
        <View className="flex-1 w-full justify-between bg-azul-claro-secundario pt-[15%] overflow-hidden">
            <View className="items-center justify-center flex-1 px-6">
                <View className="w-full flex-row justify-center gap-2 h-fit pb-8">
                    <BarraPesquisa />
                </View>

                <View className="flex-row justify-center gap-8 pb-9 h-fit">
                    <StatusConformidade {...statusRegulares} />
                    <StatusConformidade {...statusInconformes} />
                </View>

                <View className="flex-row justify-center gap-8 h-fit">
                    <CardCarteirasPerfil
                        onPress={() => navegarParaCategoria("Moderado")}
                        perfil="Moderado"
                        quantidade={carteirasPorPerfil.Moderado.quantidade}
                        total={carteirasPorPerfil.Moderado.total}
                    />
                    <CardCarteirasPerfil
                        onPress={() => navegarParaCategoria("Sofisticado")}
                        perfil="Sofisticado"
                        quantidade={carteirasPorPerfil.Sofisticado.quantidade}
                        total={carteirasPorPerfil.Sofisticado.total}
                    />
                </View>

                <View className="flex-row justify-center gap-8 mt-8">
                    <CardCarteirasPerfil
                        onPress={() => navegarParaCategoria("Conservador")}
                        perfil="Conservador"
                        quantidade={carteirasPorPerfil.Conservador.quantidade}
                        total={carteirasPorPerfil.Conservador.total}
                    />
                    <CardCarteirasPerfil
                        onPress={() => navegarParaCategoria("Todas")}
                        perfil="Todas"
                        quantidade={carteirasPorPerfil.Todas.quantidade}
                        total={carteirasPorPerfil.Todas.total}
                    />
                </View>
            </View>
        </View>
    );
}
