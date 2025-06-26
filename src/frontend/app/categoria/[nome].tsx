import { RelativePathString, useLocalSearchParams, useRouter } from "expo-router";
import { Image, Text, View, ScrollView, TouchableOpacity } from "react-native";
import { ClientesProvider, useClientes } from "../contexts/ClientesProvider";
import { useCarteiras } from "../contexts/CarteirasProvider";
import BarraPesquisa from "@/components/barraPesquisa";
import BotaoFiltro from "@/components/botaoFiltro";

function CategoriaContent() {
    const { nome } = useLocalSearchParams();
    const router = useRouter();
    const { clientes } = useClientes();

    // Fallback defensivo para evitar erro se clientes for undefined
    const clientesArray = Array.isArray(clientes) ? clientes : [];

    const { carteirasPorPerfil, carteiras } = useCarteiras();
    const dadosCategoria = carteirasPorPerfil[nome as keyof typeof carteirasPorPerfil];
    const dadosStatus = carteiras[nome as keyof typeof carteiras];

    const clientesFiltrados = clientesArray.filter((cliente) => {
        if (nome === "Todas") return true;
        if (nome === "Regulares") return cliente.status === "Regular";
        if (nome === "Inconformes") return cliente.status === "Inconforme";
        return cliente.perfilInvestidor === nome;
    });

    const navegarParaCliente = (id: string) => {
        router.push(`/investor/${id}` as RelativePathString);
    };

    return (
        <View className="relative bg-azul-claro-secundario h-full w-full">
            <View className="absolute top-0 w-full h-44 justify-center items-start">
                <Image
                    source={require("@/assets/head.png")}
                    className="w-full absolute top-[-90px]"
                />
                <Text className="font-light text-azul-claro-principal text-xl pl-6">Categoria</Text>
                <Text className="text-azul-claro-principal text-3xl font-bold pl-6">{nome}</Text>
            </View>
            <View className="w-full flex-1 items-center justify-center px-6 pt-14">
                <View className="w-full flex-row justify-center gap-2 h-fit pb-8">
                    <BarraPesquisa />
                    <BotaoFiltro />
                </View>
                <View className="bg-azul-escuro-principal rounded-2xl w-full h-[68%] p-4">
                    <View className="text-sm text-azul-claro-principal w-full justify-between flex-row items-center mb-2">
                        <Text className="text-sm text-azul-claro-principal">Carteiras</Text>
                        <Text className="bg-azul-claro-principal w-fit py-1 px-3 rounded-full text-[12px] font-light">
                            <Text className="text-sm font-bold">{(dadosCategoria?.quantidade ?? dadosStatus?.quantidade) || 0} </Text>
                            de
                            <Text className="text-sm font-bold"> {(dadosCategoria?.total ?? dadosStatus?.total) || 0} </Text>
                            carteiras nesta categoria
                        </Text>
                    </View>
                    <ScrollView>
                        {clientesFiltrados.map((cliente) => (
                            <TouchableOpacity
                                key={cliente.id}
                                className="bg-white p-4 my-2 rounded-xl"
                                onPress={() => navegarParaCliente(cliente.id)}
                            >
                                <Text className="text-base font-bold text-azul-escuro-principal">{cliente.nome}</Text>
                                <Text className="text-sm text-zinc-500">{cliente.perfilInvestidor} - {cliente.status}</Text>
                            </TouchableOpacity>
                        ))}
                    </ScrollView>
                </View>
            </View>
        </View>
    );
}

export default function Categoria() {
    const { nome } = useLocalSearchParams();

    let tipoRisco = "";

    if (nome === "Todas") {
        tipoRisco = "geral";
    } else if (nome === "Regulares" || nome === "Inconformes") {
        tipoRisco = "geral"; // ou você pode omitir o Provider
    } else {
        tipoRisco = nome as string;
    }

    return (
        <ClientesProvider tipoRisco={tipoRisco}>
            <CategoriaContent />
        </ClientesProvider>
    );
}
