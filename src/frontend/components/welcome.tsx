import { Image, View, Text } from "react-native";

interface WelcomeProps {
    user: string,
}

export default function Welcome({user}:WelcomeProps) {
    return (
        <View className="h-fit fixed">
            <View className="relative">
                <Image
                    source={require("../assets/head.png")}
                    className="top-[-90px]"
                />
                <Text className="absolute top-20 left-8 text-azul-claro-principal text-3xl font-light">
                    Bem vindo,
                    <Text className="font-bold"> {user}
                        <Text className="text-rosa-secundario">.</Text>
                    </Text>
                </Text>
            </View>
        </View>
    )
} 