import { useLocalSearchParams } from "expo-router";
import { View, Text, Image } from "react-native";
import BarraPesquisa from "@/components/barraPesquisa";
import BotaoFiltro from "@/components/botaoFiltro";

export default function Categoria() {
    const { nome } = useLocalSearchParams();

    return (
        <View className="relative bg-azul-claro-secundario h-full w-full">
            <View className="absolute top-0 w-full h-44 justify-center items-start">
                <Image
                    source={require("@/assets/head.png")}
                    className="w-full absolute top-[-90px]"
                />
                <Text className="font-light text-azul-claro-principal text-xl pl-6">Categoria</Text>
                <Text className="text-azul-claro-principal text-3xl font-bold pl-6">{nome}</Text>
            </View>``
            <View className="w-full flex-1 items-center justify-center px-6 pt-14">
                <View className="w-full flex-row justify-center gap-2 h-fit pb-8">
                    <BarraPesquisa />
                    <BotaoFiltro />
                </View>
                <View className="bg-azul-escuro-principal rounded-2xl w-full h-[68%] p-4">
                    <View className="text-sm text-azul-claro-principal w-full justify-between flex-row items-center">
                        <Text className="text-sm text-azul-claro-principal">Carteiras</Text>
                        <Text className="bg-azul-claro-principal w-fit py-1 px-3 rounded-full text-[12px] font-light">
                            <Text className="text-sm font-bold">30 </Text>
                            de 
                            <Text className="text-sm font-bold"> 40 </Text>
                            carteiras em conformidade
                        </Text>
                    </View>
                    <View>

                    </View>
                </View>
            </View>
        </View>
    );
}
