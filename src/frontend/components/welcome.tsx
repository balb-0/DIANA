import { Image, View, Text } from "react-native";

interface WelcomeProps {
    user: string,
}

export default function Welcome({user}: WelcomeProps) {
    return (
        <View className="top-0 absolute w-full z-10">
            <View className="relative">
                <Image
                    source={require("../assets/head.png")}
                    className="top-[-90px]"
                    testID="welcome-image"
                />
                <Text testID="welcome-text" className="absolute top-20 left-8 text-azul-claro-principal text-3xl font-light">
                    Bem vindo,
                    <Text testID="user-name" className="font-bold"> {user}
                        <Text testID="welcome-dot" className="text-rosa-secundario">.</Text>
                    </Text>
                </Text>
            </View>
        </View>
    )
}
