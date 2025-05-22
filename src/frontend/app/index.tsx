import React from "react";
import { View, Text } from "react-native"
import Teste from "@/components/teste";

export default function Index() {
    return(
        <View className="w-screen h-screen items-center justify-center gap-10">
            <Text className="text-red-400 text-2xl">Oi né rs</Text>
            <Teste />
        </View>
    )
}